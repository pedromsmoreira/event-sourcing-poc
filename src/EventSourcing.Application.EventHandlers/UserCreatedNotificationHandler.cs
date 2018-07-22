namespace EventSourcing.Application.EventHandlers
{
    using System.Threading.Tasks;

    using Events.Users;

    using Infrastructure.Mediator;

    using Persistence.ElasticSearch.Users;

    using Queries.Users;

    public class UserCreatedNotificationHandler : INotificationHandlerAsync<UserCreatedV1>
    {
        private readonly UsersElasticSearchRepository repository;
        private readonly IUsersQueryHandlerAsync queryHandlerAsync;

        public UserCreatedNotificationHandler(UsersElasticSearchRepository repository, IUsersQueryHandlerAsync queryHandlerAsync)
        {
            this.repository = repository;
            this.queryHandlerAsync = queryHandlerAsync;
        }

        public async Task HandleAsync(UserCreatedV1 notification)
        {
            var user = await this.queryHandlerAsync.HandleAsync(new GetUserByIdQuery(notification.Id)).ConfigureAwait(false);

            await this.repository.InsertAsync(user).ConfigureAwait(false);
        }
    }
}