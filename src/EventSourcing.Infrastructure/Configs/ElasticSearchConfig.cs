namespace EventSourcing.Infrastructure.Configs
{
    public class ElasticSearchConfig
    {
        public string ConnectionString { get; set; }

        public int Port { get; set; }
    }
}