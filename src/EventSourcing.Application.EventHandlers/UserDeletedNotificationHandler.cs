namespace EventSourcing.Application.EventHandlers
{
    using System.Threading.Tasks;

    using Events.Users;

    using Infrastructure.Mediator;

    using Persistence.ElasticSearch.Users;

    using Queries.Users;

    public class UserDeletedNotificationHandler : INotificationHandlerAsync<UserDeletedV1>
    {
        private readonly UsersElasticSearchRepository repository;
        private readonly IUsersQueryHandlerAsync usersQueryHandlerAsync;

        public UserDeletedNotificationHandler(UsersElasticSearchRepository repository, IUsersQueryHandlerAsync usersQueryHandlerAsync)
        {
            this.repository = repository;
            this.usersQueryHandlerAsync = usersQueryHandlerAsync;
        }

        public async Task HandleAsync(UserDeletedV1 notification)
        {
            var user = await this.usersQueryHandlerAsync.HandleAsync(new GetUserByIdQuery(notification.Id)).ConfigureAwait(false);

            if (user == null)
            {
                return;
            }

            await this.repository.DeleteAsync(user).ConfigureAwait(false);
        }
    }
}