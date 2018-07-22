namespace EventSourcing.Application.Queries.Events
{
    using System;
    using Infrastructure.Queries;

    public class GetEventsForAggregateId : IQuery
    {
        public GetEventsForAggregateId(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}