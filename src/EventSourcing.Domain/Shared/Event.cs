namespace EventSourcing.Domain.Shared
{
    using Infrastructure.Events;
    using Infrastructure.Shared;

    public class Event : IDomainEvent
    {
        public Event()
        {
            this.Timestamp = new Timestamp().UnixTimeEpochTimestamp;
        }

        public int Version;

        public long Timestamp { get; set; }
    }
}