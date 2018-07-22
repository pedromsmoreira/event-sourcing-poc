namespace EventSourcing.Application.EventHandlers
{
    using System.Threading.Tasks;

    using Events.Users;

    using Infrastructure.Mediator;

    using Persistence.ElasticSearch.Users;

    using Queries.Users;

    public class UserUpdatedNotificationHandler : INotificationHandlerAsync<UserUpdatedV1>
    {
        private readonly UsersElasticSearchRepository repository;
        private readonly IUsersQueryHandlerAsync usersQueryHandlerAsync;

        public UserUpdatedNotificationHandler(UsersElasticSearchRepository repository, IUsersQueryHandlerAsync usersQueryHandlerAsync)
        {
            this.repository = repository;
            this.usersQueryHandlerAsync = usersQueryHandlerAsync;
        }

        public async Task HandleAsync(UserUpdatedV1 notification)
        {
            var user = await this.usersQueryHandlerAsync.HandleAsync(new GetUserByIdQuery(notification.Id)).ConfigureAwait(false);

            if (user == null)
            {
                return;
            }

            await this.repository.UpdateAsync(user).ConfigureAwait(false);
        }
    }
}