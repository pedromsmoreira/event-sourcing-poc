namespace EventSourcing.Application.Events.Users
{
    using System;

    using Infrastructure.Mediator;

    // TODO: Add Metadata to events StreamId, Timestamp, proper versioning
    public class UserCreatedV1 : INotification
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }

        public int Version { get; set; } = 1;
    }
}