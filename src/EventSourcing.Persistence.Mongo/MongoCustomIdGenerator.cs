namespace EventSourcing.Persistence.Mongo
{
    using System;

    using MongoDB.Bson.Serialization;

    public class MongoCustomIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document) => Guid.NewGuid().ToString();

        public bool IsEmpty(object id) => string.IsNullOrEmpty(id?.ToString());
    }
}