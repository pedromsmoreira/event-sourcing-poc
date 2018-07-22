namespace EventSourcing.Application.Queries.Events
{
    using System.Collections.Generic;
    using Domain.Events;
    using Infrastructure.Events;
    using Infrastructure.Queries;

    public interface IEventsQueryHandlerAsync :
        IQueryHandlerAsync<GetAllEventsQuery, IEnumerable<IDomainEvent>>,
        IQueryHandlerAsync<GetEventsForAggregateId, IEnumerable<IDomainEvent>>,
        IQueryHandlerAsync<GetAllEventStreamsQuery, IEnumerable<EventStream>>,
        IQueryHandlerAsync<GetEventStreamById, EventStream>
    {
    }
}