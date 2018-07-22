namespace WriteModel.Integration.Tests.Shared.Data
{
    using System;
    using System.Threading.Tasks;

    using EventSourcing.Infrastructure.Configs;
    using EventSourcing.Persistence.Mongo;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoDbEventStore
    {
        private readonly IMongoClient client;

        private readonly IMongoCollection<EventDescription> collection;

        public MongoDbEventStore(MongoDbConfig settings)
        {
            this.client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(settings.ConnectionString, settings.Port),
                Credential = MongoCredential.CreateCredential(settings.DatabaseName, settings.User, settings.Password)
            });

            var db = this.client.GetDatabase(settings.DatabaseName);

            this.collection = db.GetCollection<EventDescription>("eventDescriptions", new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            });
        }

        public async Task DeleteEvents(Guid aggregateId)
        {
            await this.collection.DeleteManyAsync(Builders<EventDescription>.Filter.Eq("AggregateId", aggregateId.ToString()));
        }
    }
}