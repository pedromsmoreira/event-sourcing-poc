namespace EventSourcing.Data.Model
{
    using System;

    public class UserIndex
    {
        public string EventStreamId { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }

        public DateTime LastSnapshot { get; set; }

        public string Action { get; set; }

        public bool IsDeleted { get; set; }
    }
}