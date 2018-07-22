namespace EventSourcing.Infrastructure.Configs
{
    public class AppSettings
    {
        public MongoDbConfig MongoDb { get; set; }

        public ElasticSearchConfig ElasticSearch { get; set; }
    }
}