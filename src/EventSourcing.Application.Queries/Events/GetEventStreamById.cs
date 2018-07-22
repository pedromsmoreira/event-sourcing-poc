namespace EventSourcing.Application.Queries.Events
{
    using System;
    using Infrastructure.Queries;

    public class GetEventStreamById : IQuery
    {
        public GetEventStreamById(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}