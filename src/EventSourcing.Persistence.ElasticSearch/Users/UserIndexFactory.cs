namespace EventSourcing.Persistence.ElasticSearch.Users
{
    using System;

    using Data.Model;

    using Domain.Users;

    public static class UserIndexFactory
    {
        public static UserIndex CreateSimpleIndex(User user, UserAction action, bool isDeleted = false)
        {
            return new UserIndex
            {
                Id = Guid.Parse(user.Id),
                Name = user.Name,
                Job = user.Job,
                LastSnapshot = DateTime.UtcNow,
                Action = action.ToString(),
                IsDeleted = isDeleted
            };
        }
    }
}