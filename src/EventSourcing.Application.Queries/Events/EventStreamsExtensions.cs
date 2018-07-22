namespace EventSourcing.Application.Queries.Events
{
    using System.Collections.Generic;
    using System.Linq;

    using Domain.Events;

    using DataModel = Persistence.Mongo.EventStreamStore;

    public static class EventStreamsExtensions
    {
        public static EventStream MapStream(this DataModel.EventStream stream)
        {
            return new EventStream
            {
                Events = stream.Events,
                EventStreamId = stream.EventStreamId,
                Timestamp = stream.Timestamp
            };
        }

        public static IReadOnlyList<EventStream> MapStreams(this IEnumerable<DataModel.EventStream> stream)
        {
            return stream.Select(MapStream).ToList();
        }
    }
}