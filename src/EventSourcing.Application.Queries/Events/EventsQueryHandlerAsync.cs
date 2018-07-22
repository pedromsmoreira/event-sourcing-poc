namespace EventSourcing.Application.Queries.Events
{
    using System.Collections.Generic;
    using System.IO.MemoryMappedFiles;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Events;

    using Infrastructure.Events;

    using Persistence.Mongo;

    public class EventsQueryHandlerAsync : IEventsQueryHandlerAsync
    {
        private readonly IEventStore eventStore;

        public EventsQueryHandlerAsync(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(GetAllEventsQuery query)
        {
            return await this.eventStore.GetEvents().ConfigureAwait(false);
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(GetEventsForAggregateId query)
        {
            return await this.eventStore.GetEventsForAggregate(query.Id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<EventStream>> HandleAsync(GetAllEventStreamsQuery query)
        {
            var eventStreams = await this.eventStore.GetEventStreams().ConfigureAwait(false);

            return !eventStreams.Any() ? new List<EventStream>() : eventStreams.MapStreams();
        }

        public async Task<EventStream> HandleAsync(GetEventStreamById query)
        {
            var stream = await this.eventStore.GetEventStreamById(query.Id).ConfigureAwait(false);

            return stream?.MapStream();
        }
    }
}