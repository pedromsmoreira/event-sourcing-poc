namespace EventSourcing.WriteModel.Events
{
    using System;
    using System.Threading.Tasks;

    using Application.Queries.Events;

    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    public class EventsController : Controller
    {
        private readonly IEventsQueryHandlerAsync queryHandlerAsync;

        public EventsController(IEventsQueryHandlerAsync queryHandlerAsync)
        {
            this.queryHandlerAsync = queryHandlerAsync;
        }

        [HttpGet]
        [Route("events/{aggregateId}")]
        public async Task<IActionResult> Get([FromRoute]Guid aggregateId)
        {
            var result = await this.queryHandlerAsync.HandleAsync(new GetEventsForAggregateId(aggregateId));

            return this.Ok(result);
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> Get()
        {
            var result = await this.queryHandlerAsync.HandleAsync(new GetAllEventsQuery()).ConfigureAwait(false);

            return this.Ok(result);
        }

        [HttpGet]
        [Route("streams")]
        public async Task<IActionResult> GetEventStreams()
        {
            var query = new GetAllEventStreamsQuery();
            var result = await this.queryHandlerAsync.HandleAsync(query).ConfigureAwait(false);

            return this.Ok(result);
        }

        [HttpGet]
        [Route("streams/{eventStreamId}")]
        public async Task<IActionResult> GetEventStreams([FromRoute]Guid eventStreamId)
        {
            var query = new GetEventStreamById(eventStreamId);
            var result = await this.queryHandlerAsync.HandleAsync(query).ConfigureAwait(false);

            return this.Ok(result);
        }
    }
}