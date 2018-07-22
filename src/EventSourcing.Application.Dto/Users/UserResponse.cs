namespace EventSourcing.Application.Dto.Users
{
    using System;

    public class UserResponse
    {
        public string Name { get; set; }

        public string Job { get; set; }

        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}