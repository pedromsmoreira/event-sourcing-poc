namespace EventSourcing.Persistence.Mongo.EventStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Infrastructure.Configs;
    using Infrastructure.Events;

    using Microsoft.Extensions.Options;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Users;

    public class EventStreamEventStore : IEventStore
    {
        private const string CollectionName = "eventStreamStore";

        private static readonly MongoCollectionSettings collectionOptions = new MongoCollectionSettings
        {
            GuidRepresentation = GuidRepresentation.CSharpLegacy
        };

        private readonly IMongoCollection<EventStream> collection;

        public EventStreamEventStore(IOptions<MongoDbConfig> settings)
        {
            var config = settings.Value;

            UserEventsClassMapRegistry.LoadClassMapRegistry();

            var client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(config.ConnectionString, config.Port),
                Credential = MongoCredential.CreateCredential(config.DatabaseName, config.User, config.Password)
            });

            var db = client.GetDatabase(config.DatabaseName);

            if (CollectionExists(db, CollectionName))
            {
                this.collection = db.GetCollection<EventStream>(CollectionName, collectionOptions);
            }
            else
            {
                db.CreateCollection(CollectionName);
                this.collection = db.GetCollection<EventStream>(CollectionName, collectionOptions);
            }
        }

        public async Task SaveEvents(Guid aggregateId, IReadOnlyList<IDomainEvent> uncommittedChanges)
        {
            if (!uncommittedChanges.Any())
            {
                return;
            }

            var eventStream = await this.collection
                .Find(Builders<EventStream>.Filter.Eq(es => es.EventStreamId, aggregateId.ToString()))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (eventStream == null)
            {
                eventStream = new EventStream(aggregateId.ToString());

                await this.collection.InsertOneAsync(eventStream).ConfigureAwait(false);
            }

            foreach (var uncommittedChange in uncommittedChanges)
            {
                eventStream.Events.Add(uncommittedChange);
            }

            await this.collection.UpdateOneAsync(
                Builders<EventStream>.Filter.Eq(es => es.EventStreamId, aggregateId.ToString()),
                Builders<EventStream>.Update.Set(es => es.Events, eventStream.Events));
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEventsForAggregate(Guid aggregateId)
        {
            var eventStream = await this.collection
                .Find(Builders<EventStream>.Filter.Eq(es => es.EventStreamId, aggregateId.ToString()))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return eventStream.Events;
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEvents()
        {
            var eventsList = new List<IDomainEvent>();

            await this.collection
                .Find(_ => true)
                .ForEachAsync(eventStream => eventsList.AddRange(eventStream.Events))
                .ConfigureAwait(false);

            return eventsList;
        }

        public async Task<IReadOnlyList<EventStream>> GetEventStreams()
        {
            return await this.collection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<EventStream> GetEventStreamById(Guid aggregateId)
        {
            var eventStream = await this.collection
                .Find(Builders<EventStream>.Filter.Eq(es => es.EventStreamId, aggregateId.ToString()))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return eventStream;
        }

        private static bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);

            var options = new ListCollectionsOptions
            {
                Filter = filter
            };

            return database.ListCollections(options).Any();
        }
    }
}