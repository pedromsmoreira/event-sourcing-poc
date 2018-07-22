namespace EventSourcing.Domain.Events
{
    using System.Collections.Generic;

    using Infrastructure.Events;

    public class EventStream
    {
        public List<IDomainEvent> Events { get; set; }

        public string EventStreamId { get; set; }

        public long Timestamp { get; set; }
    }
}