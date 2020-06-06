namespace EventSourcing.Application.Events
{
    using System.Threading.Tasks;

    using Events.Users;

    using EventSourcing.Application.Queries.Users;
    using EventSourcing.Persistence.ElasticSearch.Users;

    using Infrastructure.Mediator;

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