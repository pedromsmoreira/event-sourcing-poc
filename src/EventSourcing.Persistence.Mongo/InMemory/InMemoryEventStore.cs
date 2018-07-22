namespace EventSourcing.Persistence.Mongo.InMemory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Infrastructure.Events;
    using Infrastructure.Exceptions;

    public class InMemoryEventStore// : IEventStore
    {
        private struct EventDescription
        {
            // TODO: Add version later...
            public EventDescription(Guid id, IDomainEvent eventData)
            {
                this.Id = id;
                this.EventData = eventData;
            }

            public IDomainEvent EventData { get; }

            public Guid Id { get; }
        }

        private readonly IDictionary<Guid, List<EventDescription>> current = new Dictionary<Guid, List<EventDescription>>();

        public Task SaveEvents(Guid aggregateId, IEnumerable<IDomainEvent> uncommittedChanges)
        {
            if (!this.current.TryGetValue(aggregateId, out var eventDescriptors))
            {
                eventDescriptors = new List<EventDescription>();

                this.current.Add(aggregateId, eventDescriptors);
            }

            eventDescriptors.AddRange(uncommittedChanges.Select(uncommittedChange => new EventDescription(aggregateId, uncommittedChange)));

            return Task.CompletedTask;
        }

        public Task<IEnumerable<IDomainEvent>> GetEventsForAggregate(Guid aggregateId)
        {
            if (!this.current.TryGetValue(aggregateId, out var eventDescriptors))
            {
                throw new AggregateNotFoundException();
            }

            return Task.FromResult(eventDescriptors.Select(desc => desc.EventData));
        }

        //public Task<IEnumerable<IDomainEvent>> GetEvents()
        //{
        //    //if (!this.current.TryGetValue(aggregateId, out var eventDescriptors))
        //    //{
        //    //    throw new AggregateNotFoundException();
        //    //}

        //    //return Task.FromResult(eventDescriptors.Select(desc => desc.EventData));
        //}
    }
}