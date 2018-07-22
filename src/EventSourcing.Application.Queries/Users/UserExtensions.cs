namespace EventSourcing.Application.Queries.Users
{
    using System;

    using Domain.Users;

    using Dto.Users;

    public static class UserExtensions
    {
        public static UserResponse ToDto(this User user)
        {
            return new UserResponse
            {
                Name = user.Name,
                Id = Guid.Parse(user.Id),
                Job = user.Job,
                IsDeleted = user.IsDeleted
            };
        }
    }
}