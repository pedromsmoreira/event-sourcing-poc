namespace Domain.Tests
{
    using System;
    using System.Linq;

    using EventSourcing.Domain.Users;
    using EventSourcing.Domain.Users.Events;

    using FluentAssertions;

    using Xunit;

    public class UserTests
    {
        [Fact]
        public void CreateUser_ValidArguments_ShouldHaveOneUncommitedChange()
        {
            // Act
            var sut = new User("name", "job");

            // Assert
            sut.GetUncommitedChanges().Should().HaveCount(1);
        }

        [Fact]
        public void ExistingUser_UpdateName_ShouldHaveOneUncommitedChange()
        {
            // Arrange
            var sut = new User("name", "job");
            sut.MarkChangesAsCommited();

            // Act
            sut.ChangeName("name updated");

            // Assert
            sut.GetUncommitedChanges().Should().HaveCount(1);
        }

        [Fact]
        public void ExistingUser_UpdateJob_ShouldHaveOneUncommitedChange()
        {
            // Arrange
            var sut = new User("name", "job");
            sut.MarkChangesAsCommited();

            // Act
            sut.ChangeJob("job updated");

            // Assert
            sut.GetUncommitedChanges().Should().HaveCount(1);
        }

        [Fact]
        public void ExistingUser_UpdateJobAndName_ShouldHaveTwoUncommitedChanges()
        {
            // Arrange
            var sut = new User("name", "job");
            sut.MarkChangesAsCommited();

            // Act
            sut.ChangeJob("job updated");
            sut.ChangeName("name updated");

            // Assert
            sut.GetUncommitedChanges().Should().HaveCount(2);
        }

        [Fact]
        public void ExistingUser_LoadFromHistory_ShouldHaveUpdatedValues()
        {
            // Arrange
            const string updatedName = "updated name";
            const string updatedJob = "updated job";

            var sut = new User("name", "job");

            sut.ChangeJob(updatedJob);
            sut.ChangeName(updatedName);

            // Act
            sut.LoadFromHistory(sut.GetUncommitedChanges());

            // Assert
            sut.Name.Should().Be(updatedName);
            sut.Job.Should().Be(updatedJob);
        }

        [Fact]
        public void ExistingUser_NullNameWhenChangingName_ShouldThrowArgumentException()
        {
            // Arrange
            var sut = new User("name", "job");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.ChangeName(null));
        }

        [Fact]
        public void ExistingUser_NullJobWhenChangingJob_ShouldThrowArgumentException()
        {
            // Arrange
            var sut = new User("name", "job");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.ChangeJob(null));
        }

        [Fact]
        public void ExistingUser_MarkUserAsDeleted_ShouldHaveUserDeletedMessage()
        {
            // Arrange
            var sut = new User("name", "job");
            sut.MarkChangesAsCommited();

            // Act
            sut.MarkAsDeleted();

            // Assert
            sut.GetUncommitedChanges().Should().HaveCount(1);
            sut.GetUncommitedChanges().FirstOrDefault().Should().BeOfType<UserDeleted>();
        }

        [Fact]
        public void CreateUser_UserShouldNotBeMarkedAsDeleted()
        {
            // Act
            var sut = new User("name", "job");

            // Assert
            sut.LoadFromHistory(sut.GetUncommitedChanges());
            sut.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public void ExistingUser_MarkAsDeleted_ShouldBeMarkedAsDeleted()
        {
            // Arrange
            var sut = new User("name", "job");

            // Act
            sut.MarkAsDeleted();

            // Assert
            sut.LoadFromHistory(sut.GetUncommitedChanges());
            sut.IsDeleted.Should().BeTrue();
        }
    }
}