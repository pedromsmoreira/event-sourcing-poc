namespace WriteModel.Integration.Tests.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using EventSourcing.Application.Dto.Users;
    using EventSourcing.WriteModel;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;

    using Shared;

    using Xunit;

    public class UsersTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture dbFixture;

        public UsersTests(TestServerFixture dbFixture)
        {
            this.dbFixture = dbFixture;
        }

        [Fact]
        public async Task PostUser_ValidUser_ShouldReturnId()
        {
            // Arrange
            var request = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Create User")
                .Build();

            // Act
            var sut = await this.dbFixture.Client.PostAsJsonAsync("/users", request);
            sut.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await sut.Content.ReadAsStringAsync());

            // Assert
            createdId.Should().NotBeEmpty();

            var getUser = await this.dbFixture.Client.GetAsync($"/users/{createdId}");
            var content = await getUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserResponse>(content);

            user.Should().NotBeNull();
            user.Name.Should().Be(request.Name);
            user.Job.Should().Be(request.Job);

            await this.CleanUp(createdId).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateUser_ChangeUserNameAndJob_ShouldHaveTwoEvents()
        {
            // Arrange
            var createRequest = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Update User Name And Job")
                .Build();

            var updateRequest = Builder<UpdateUserRequest>
                .CreateNew()
                .With(u => u.Name = $"{createRequest.Name} updated")
                .With(u => u.Job = $"{createRequest.Job} updated")
                .Build();

            var setup = await this.dbFixture.Client.PostAsJsonAsync("/users", createRequest);
            setup.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await setup.Content.ReadAsStringAsync());
            this.dbFixture.IdsToDelete.Add(createdId);

            // Act
            var sut = await this.dbFixture.Client.PutAsJsonAsync($"/users/{createdId}", updateRequest);
            sut.EnsureSuccessStatusCode();

            var responseString = JsonConvert.DeserializeObject<Guid>(await sut.Content.ReadAsStringAsync());

            // Assert
            responseString.Should().NotBeEmpty();
            var getUser = await this.dbFixture.Client.GetAsync($"/users/{responseString}");
            var user = JsonConvert.DeserializeObject<UserResponse>(await getUser.Content.ReadAsStringAsync());

            user.Should().NotBeNull();
            user.Name.Should().Be(updateRequest.Name);
            user.Job.Should().Be(updateRequest.Job);

            await this.CleanUp(createdId).ConfigureAwait(false);
        }

        [Fact]
        public async Task DeleteUser_GetUser_ShouldReturnUserMarkedAsDeleted()
        {
            // Arrange
            var createRequest = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Delete User")
                .Build();

            var setup = await this.dbFixture.Client.PostAsJsonAsync("/users", createRequest);
            setup.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await setup.Content.ReadAsStringAsync());
            this.dbFixture.IdsToDelete.Add(createdId);

            // Act
            var sut = await this.dbFixture.Client.DeleteAsync($"/users/{createdId}");

            sut.EnsureSuccessStatusCode();

            var getUser = await this.dbFixture.Client.GetAsync($"/users/{createdId}");
            var user = JsonConvert.DeserializeObject<UserResponse>(await getUser.Content.ReadAsStringAsync());

            // Assert
            user.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUser_SearchUser_ShouldReturnAtLeastOneUser()
        {
            // Arrange
            var createRequest = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Created User for Search")
                .Build();

            // Act
            var sut = await this.dbFixture.Client.PostAsJsonAsync("/users", createRequest);
            sut.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await sut.Content.ReadAsStringAsync());

            // Assert
            createdId.Should().NotBeEmpty();

            var search = await this.dbFixture.Client.GetAsync($"/users/search?name={createRequest.Name}");
            var searchResults = JsonConvert.DeserializeObject<IReadOnlyCollection<UserSearchResponse>>(await search.Content.ReadAsStringAsync());

            // Assert
            searchResults.Should().HaveCountGreaterOrEqualTo(1, $"Search didn't return results for {createRequest.Name}");

            await this.CleanUp(createdId).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateUser_SearchUser_ShouldReturnUpdatedUser()
        {
            // Arrange
            var createRequest = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Update User for Search")
                .Build();

            var updateRequest = Builder<UpdateUserRequest>
                .CreateNew()
                .With(u => u.Name = $"{createRequest.Name} updated")
                .With(u => u.Job = $"{createRequest.Job} updated")
                .Build();

            var setup = await this.dbFixture.Client.PostAsJsonAsync("/users", createRequest);
            setup.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await setup.Content.ReadAsStringAsync());
            this.dbFixture.IdsToDelete.Add(createdId);

            // Act
            var sut = await this.dbFixture.Client.PutAsJsonAsync($"/users/{createdId}", updateRequest);
            sut.EnsureSuccessStatusCode();

            var responseString = JsonConvert.DeserializeObject<Guid>(await sut.Content.ReadAsStringAsync());

            responseString.Should().NotBeEmpty();

            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            var search = await this.dbFixture.Client.GetAsync($"/users/search?name={updateRequest.Name}&job={updateRequest.Job}");
            var searchResults = JsonConvert.DeserializeObject<IReadOnlyCollection<UserSearchResponse>>(await search.Content.ReadAsStringAsync());

            // Assert
            searchResults.Should().HaveCountGreaterOrEqualTo(1, $"Search didn't return results for {updateRequest.Name}");
            var user = searchResults.FirstOrDefault(u => u.Id.Equals(responseString));

            user.Name.Should().Be(updateRequest.Name);

            await this.CleanUp(createdId).ConfigureAwait(false);
        }

        [Fact]
        public async Task DeleteUser_SearchUser_ShouldNotReturnDeletedUser()
        {
            // Arrange
            var createRequest = Builder<CreateUserRequest>
                .CreateNew()
                .With(u => u.Name = "Delete User for Search")
                .Build();

            var setup = await this.dbFixture.Client.PostAsJsonAsync("/users", createRequest);
            setup.EnsureSuccessStatusCode();

            var createdId = JsonConvert.DeserializeObject<Guid>(await setup.Content.ReadAsStringAsync());
            this.dbFixture.IdsToDelete.Add(createdId);

            // Act
            var sut = await this.dbFixture.Client.DeleteAsync($"/users/{createdId}");

            sut.EnsureSuccessStatusCode();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            var search = await this.dbFixture.Client.GetAsync($"/users/search?name={createRequest.Name}");
            var searchResults = JsonConvert.DeserializeObject<IReadOnlyCollection<UserSearchResponse>>(await search.Content.ReadAsStringAsync());

            // Assert
            searchResults
                .FirstOrDefault(
                    u => u.Id.Equals(createdId))
                .Should().BeNull();
        }

        private async Task CleanUp(Guid createdId)
        {
            this.dbFixture.IdsToDelete.Add(createdId);
            await this.dbFixture.Client.DeleteAsync($"/users/{createdId}");
        }
    }
}