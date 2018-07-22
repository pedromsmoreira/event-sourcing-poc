namespace EventSourcing.Application.Events.Users
{
    using System;
    using Infrastructure.Mediator;

    public class UserUpdatedV1 : INotification
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }

        public int Version { get; set; } = 1;

        public string EventStreamId { get; set; }
    }
}