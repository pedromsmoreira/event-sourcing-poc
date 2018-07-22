namespace EventSourcing.Persistence.ElasticSearch
{
    using System;

    public class EventMetadata<T>
    {
        public string EventStreamId { get; set; }

        public T Content { get; set; }

        public DateTime LastSnapshot { get; set; }
    }
}