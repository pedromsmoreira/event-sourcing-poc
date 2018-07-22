namespace EventSourcing.Persistence.Mongo.EventStreamStore
{
    using System.Collections.Generic;

    using Infrastructure.Events;
    using Infrastructure.Shared;

    using MongoDB.Bson;

    public class EventStream : IUuid<ObjectId>
    {
        public EventStream(string eventStreamId)
        {
            this.Id = ObjectId.GenerateNewId();
            this.EventStreamId = eventStreamId;
            this.Events = new List<IDomainEvent>();
            this.Timestamp = new Timestamp().UnixTimeEpochTimestamp;
        }

        public List<IDomainEvent> Events { get; set; }

        public string EventStreamId { get; set; }

        public long Timestamp { get; set; }

        public ObjectId Id { get; set; }
    }
}