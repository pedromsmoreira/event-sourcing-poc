namespace EventSourcing.Application.Commands.Users
{
    using System;
    using System.Threading.Tasks;

    using Domain.Users;

    using Events.Users;

    using Infrastructure.Mediator;

    using Persistence.Mongo;

    using Queries.Users;

    public class StoreAndPublishUsersCommandsProcessor : IUserCommandsProcessor
    {
        private readonly IEventStore eventStore;
        private readonly IUsersQueryHandlerAsync queryHandlerAsync;
        private readonly IMediator mediator;

        public StoreAndPublishUsersCommandsProcessor(
            IEventStore eventStore,
            IUsersQueryHandlerAsync queryHandlerAsync,
            IMediator mediator)
        {
            this.eventStore = eventStore;
            this.queryHandlerAsync = queryHandlerAsync;
            this.mediator = mediator;
        }

        public async Task<Guid> ProcessAsync(CreateUserCommand command)
        {
            var user = new User(command.Name, command.Job);

            Guid.TryParse(user.Id, out var aggregateId);

            await this.eventStore.SaveEvents(aggregateId, user.GetUncommitedChanges()).ConfigureAwait(false);

            var userCreatedNotification = new UserCreatedV1
            {
                Name = user.Name,
                Job = user.Job,
                Id = aggregateId,
                Version = 1
            };

            user.MarkChangesAsCommited();

            // In the future change this to a queue like Kafka
            // publish event to kafka

            // move to consumer
            await this.mediator.NotifyAsync(userCreatedNotification).ConfigureAwait(false);

            return aggregateId;
        }

        public async Task<Guid> ProcessAsync(UpdateUserCommand command)
        {
            var currentUserState = await this.queryHandlerAsync.HandleAsync(new GetUserByIdQuery(command.Id)).ConfigureAwait(false);

            currentUserState.ApplyUpdate(command);

            Guid.TryParse(currentUserState.Id, out var aggregateId);

            await this.eventStore.SaveEvents(aggregateId, currentUserState.GetUncommitedChanges()).ConfigureAwait(false);

            var userUpdatedNotification = new UserUpdatedV1
            {
                Name = currentUserState.Name,
                Job = currentUserState.Job,
                Id = aggregateId,
                Version = 1
            };

            currentUserState.MarkChangesAsCommited();

            // In the future change this to a queue like Kafka
            // publish event to kafka

            // move to consumer
            await this.mediator.NotifyAsync(userUpdatedNotification).ConfigureAwait(false);

            return aggregateId;
        }

        public async Task ProcessAsync(DeleteUserCommand command)
        {
            var user = await this.queryHandlerAsync.HandleAsync(new GetUserByIdQuery(command.Id)).ConfigureAwait(false);

            if (user.IsNull())
            {
                return;
            }

            user.MarkAsDeleted();

            await this.eventStore.SaveEvents(command.Id, user.GetUncommitedChanges()).ConfigureAwait(false);

            user.MarkChangesAsCommited();

            // In the future change this to a queue like Kafka
            // publish event to kafka

            // move to consumer
            await this.mediator.NotifyAsync(new UserDeletedV1 { Id = command.Id, Version = 1 }).ConfigureAwait(false);
        }
    }
}