namespace EventSourcing.Persistence.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CorrelationId;
    using EventStreamStore;
    using Infrastructure.Events;

    using Serilog;

    public class LoggerEventStore : IEventStore
    {
        private readonly IEventStore decorated;
        private readonly ICorrelationContextAccessor correlationContext;

        public LoggerEventStore(IEventStore decorated,
            ICorrelationContextAccessor correlationContext)
        {
            this.decorated = decorated;
            this.correlationContext = correlationContext;
        }

        public async Task SaveEvents(Guid aggregateId, IReadOnlyList<IDomainEvent> uncommittedChanges)
        {
            Log.Information(
                "Starting SaveEvents Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    AggregateId = aggregateId,
                    uncommittedChangesCount = uncommittedChanges.Count,
                    BeginningTimestamp = DateTime.UtcNow,
                    MethodName = "SaveEvents"
                });

            await this.decorated.SaveEvents(aggregateId, uncommittedChanges).ConfigureAwait(false);

            Log.Information(
                "Ending SaveEvents Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    AggregateId = aggregateId,
                    uncommittedChangesCount = uncommittedChanges.Count,
                    MethodName = "SaveEvents",
                    EndingTimestamp = DateTime.UtcNow
                });
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEventsForAggregate(Guid aggregateId)
        {
            Log.Information(
                "Starting GetEventsForAggregate Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    AggregateId = aggregateId,
                    MethodName = "GetEventsForAggregate",
                    BeginningTimestamp = DateTime.UtcNow
                });

            var events = await this.decorated.GetEventsForAggregate(aggregateId).ConfigureAwait(false);

            Log.Information("Ending GetEventsForAggregate Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    AggregateId = aggregateId,
                    EventsCount = events.Count,
                    MethodName = "GetEventsForAggregate",
                    EndingTimestamp = DateTime.UtcNow
                });

            return events;
        }

        public async Task<IReadOnlyList<IDomainEvent>> GetEvents()
        {
            Log.Information(
                "Starting GetEvents Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    MethodName = "GetEvents",
                    BeginningTimestamp = DateTime.UtcNow
                });

            var events = await this.decorated.GetEvents().ConfigureAwait(false);

            Log.Information("Ending GetEvents Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    EventsCount = events.Count,
                    MethodName = "GetEvents",
                    EndingTimestamp = DateTime.UtcNow
                });

            return events;
        }

        public async Task<IReadOnlyList<EventStream>> GetEventStreams()
        {
            Log.Information(
                "Starting GetEventStreams Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    MethodName = "GetEventStreams",
                    BeginningTimestamp = DateTime.UtcNow
                });

            var eventStreams = await this.decorated.GetEventStreams().ConfigureAwait(false);

            Log.Information("Ending GetEventStreams Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    EventStreamsCount = eventStreams.Count,
                    MethodName = "GetEventStreams",
                    EndingTimestamp = DateTime.UtcNow
                });

            return eventStreams;
        }

        public async Task<EventStream> GetEventStreamById(Guid aggregateId)
        {
            Log.Information(
                "Starting GetEventStreamById Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    MethodName = "GetEventStreamById",
                    BeginningTimestamp = DateTime.UtcNow
                });

            var eventStream = await this.decorated.GetEventStreamById(aggregateId).ConfigureAwait(false);

            Log.Information("Ending GetEventStreamById Operation. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    EventsCount = eventStream.Events.Count,
                    MethodName = "GetEventStreamById",
                    EndingTimestamp = DateTime.UtcNow
                });

            return eventStream;
        }
    }
}