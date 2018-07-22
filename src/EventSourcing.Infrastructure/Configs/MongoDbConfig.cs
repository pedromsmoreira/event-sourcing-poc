namespace EventSourcing.Infrastructure.Configs
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string DatabaseName { get; set; }
    }
}