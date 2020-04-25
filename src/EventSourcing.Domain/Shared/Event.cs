namespace EventSourcing.Domain.Shared
{
    using Infrastructure.Events;
    using Infrastructure.Shared;

    public abstract class Event : IDomainEvent
    {
        protected Event()
        {
            this.Timestamp = new Timestamp().UnixTimeEpochTimestamp;
        }

        public int Version;

        public long Timestamp { get; set; }
    }
}