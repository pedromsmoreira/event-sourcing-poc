namespace WriteModel.Integration.Tests.Shared.Data.EventStreamStore
{
    using System;
    using System.Threading.Tasks;

    using EventSourcing.Domain.Events;
    using EventSourcing.Infrastructure.Configs;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class EventStreamEventStore
    {
        private readonly IMongoCollection<EventStream> collection;

        public EventStreamEventStore(MongoDbConfig config)
        {
            var client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(config.ConnectionString, config.Port),
                Credential = MongoCredential.CreateCredential(config.DatabaseName, config.User, config.Password)
            });

            var db = client.GetDatabase(config.DatabaseName);

            this.collection = db.GetCollection<EventStream>(
                "eventStreamStore",
                new MongoCollectionSettings
                {
                    GuidRepresentation = GuidRepresentation.CSharpLegacy
                });
        }

        public async Task DeleteEventStreams(Guid aggregateId)
        {
            await this.collection.DeleteManyAsync(Builders<EventStream>.Filter.Eq(es => es.EventStreamId, aggregateId.ToString()));
        }
    }
}