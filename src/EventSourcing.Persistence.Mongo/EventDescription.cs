namespace EventSourcing.Persistence.Mongo
{
    using Infrastructure.Events;

    using MongoDB.Bson;

    public class EventDescription : IUuid<ObjectId>
    {
        public IDomainEvent EventData { get; set; }

        public string AggregateId { get; set; }

        public long Timestamp { get; set; }

        public ObjectId Id { get; set; }
    }
}