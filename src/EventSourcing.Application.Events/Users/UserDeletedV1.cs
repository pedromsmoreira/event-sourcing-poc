namespace EventSourcing.Application.Events.Users
{
    using System;

    using Infrastructure.Mediator;

    public class UserDeletedV1 : INotification
    {
        public Guid Id { get; set; }

        public int Version { get; set; } = 1;
    }
}