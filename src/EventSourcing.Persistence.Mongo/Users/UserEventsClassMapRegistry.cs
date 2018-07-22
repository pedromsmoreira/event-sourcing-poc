namespace EventSourcing.Persistence.Mongo.Users
{
    using EventSourcing.Domain.Users.Events;

    using MongoDB.Bson.Serialization;

    public static class UserEventsClassMapRegistry
    {
        public static void LoadClassMapRegistry()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserCreated)))
            {
                BsonClassMap.RegisterClassMap<UserCreated>();
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserJobChanged)))
            {
                BsonClassMap.RegisterClassMap<UserJobChanged>();
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserNameChanged)))
            {
                BsonClassMap.RegisterClassMap<UserNameChanged>();
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserDeleted)))
            {
                BsonClassMap.RegisterClassMap<UserDeleted>();
            }
        }
    }
}