namespace EventSourcing.Persistence.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EventStreamStore;

    using Infrastructure.Configs;
    using Infrastructure.Events;
    using Infrastructure.Shared;

    using Microsoft.Extensions.Options;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Users;

    public class MongoDbEventStore : IEventStore
    {
        private readonly IMongoCollection<EventDescription> collection;

        public MongoDbEventStore(IOptions<MongoDbConfig> settings)
        {
            var config = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            UserEventsClassMapRegistry.LoadClassMapRegistry();

            var client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(config.ConnectionString, config.Port),
                Credential = MongoCredential.CreateCredential(config.DatabaseName, config.User, config.Password)
            });

            var db = client.GetDatabase(config.DatabaseName);

            this.collection = db.GetCollection<EventDescription>("eventDescriptions", new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            });

            if (this.collection == null)
            {
                db.CreateCollection("eventStreamStore");
            }
        }

        public async Task SaveEvents(Guid aggregateId, IReadOnlyList<IDomainEvent> uncommittedChanges)
        {
            var eventDescriptions = new List<EventDescription>();

            if (!uncommittedChanges.Any())
            {
                return;
            }

            foreach (var uncommittedChange in uncommittedChanges)
            {
                eventDescriptions.Add(new EventDescription
                {
                    Id = ObjectId.GenerateNewId(),
                    AggregateId = aggregateId.ToString(),
                    EventData = uncommittedChange,
                    Timestamp = new Timestamp().UnixTimeEpochTimestamp
                });
            }

            await this.collection.InsertManyAsync(eventDescriptions).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEventsForAggregate(Guid aggregateId)
        {
            var eventDescriptions = new List<EventDescription>();

            await this.collection
                .Find(ed => ed.AggregateId.Equals(aggregateId.ToString()))
                .ForEachAsync(doc => eventDescriptions.Add(doc))
                .ConfigureAwait(false);

            return eventDescriptions.Select(desc => desc.EventData).ToList();
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEvents()
        {
            var eventDescriptions = new List<EventDescription>();

            await this.collection
                .Find(_ => true)
                .ForEachAsync(doc => eventDescriptions.Add(doc))
                .ConfigureAwait(false);

            return eventDescriptions.Select(desc => desc.EventData).ToList();
        }

        public Task<IReadOnlyList<EventStream>> GetEventStreams()
        {
            throw new NotImplementedException("The Limitation for this implementation of an EventStore.");
        }

        public Task<EventStream> GetEventStreamById(Guid aggregateId)
        {
            throw new NotImplementedException("The Limitation for this implementation of an EventStore.");
        }
    }
}