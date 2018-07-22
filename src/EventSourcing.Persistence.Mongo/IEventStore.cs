namespace EventSourcing.Persistence.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EventStreamStore;
    using Infrastructure.Events;

    public interface IEventStore
    {
        Task SaveEvents(Guid aggregateId, IReadOnlyList<IDomainEvent> uncommittedChanges);

        Task<IReadOnlyList<IDomainEvent>> GetEventsForAggregate(Guid aggregateId);

        Task<IReadOnlyList<IDomainEvent>> GetEvents();

        Task<IReadOnlyList<EventStream>> GetEventStreams();

        Task<EventStream> GetEventStreamById(Guid aggregateId);
    }
}