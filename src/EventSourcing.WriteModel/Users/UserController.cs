﻿namespace EventSourcing.WriteModel.Users
{
    using System;
    using System.Threading.Tasks;

    using Application.Commands.Users;
    using Application.Dto.Users;
    using Application.Queries.Users;

    using CorrelationId;

    using Microsoft.AspNetCore.Mvc;

    using Serilog;

    [Produces("application/json")]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserCommandsProcessor commandsProcessor;
        private readonly IUsersQueryHandlerAsync queryHandlerAsync;
        private readonly ICorrelationContextAccessor correlationContext;

        public UserController(
            IUserCommandsProcessor commandsProcessor,
            IUsersQueryHandlerAsync queryHandlerAsync,
            ICorrelationContextAccessor correlationContext)
        {
            this.commandsProcessor = commandsProcessor;
            this.queryHandlerAsync = queryHandlerAsync;
            this.correlationContext = correlationContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Log.Information(
                "Incoming Request for Id. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    AggregateId = id
                });

            var user = await this.queryHandlerAsync.HandleAsync(new GetUserByIdQuery(id)).ConfigureAwait(false);

            if (user.IsNull())
            {
                Log.Information(
                    "User was NOT FOUND. Data: {@data}",
                    new
                    {
                        CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                        AggregateId = id
                    });

                return this.NotFound();
            }

            Log.Information(
                "User was FOUND. User. Data: {@data}",
                new
                {
                    CorrelationId = this.correlationContext.CorrelationContext.CorrelationId,
                    Name = user.Name,
                    Job = user.Job,
                    AggregateId = user.Id,
                    IsDeleted = user.IsDeleted
                });

            return this.Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequest request)
        {
            var result = await this.commandsProcessor.ProcessAsync(new CreateUserCommand(request.Name, request.Job)).ConfigureAwait(false);

            return this.Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateUserRequest user)
        {
            var command = new UpdateUserCommand(id, user.Job, user.Name);
            var result = await this.commandsProcessor.ProcessAsync(command).ConfigureAwait(false);

            return this.Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteUserCommand(id);
            await this.commandsProcessor.ProcessAsync(command).ConfigureAwait(false);

            return this.NoContent();
        }
    }
}