namespace EventSourcing.Application.Queries.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Users;

    using Persistence.Mongo;

    public class UsersQueryHandlerAsync : IUsersQueryHandlerAsync
    {
        private readonly IEventStore eventStore;

        public UsersQueryHandlerAsync(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<User> HandleAsync(GetUserByIdQuery query)
        {
            var user = new User();

            var events = await this.eventStore.GetEventsForAggregate(query.Id).ConfigureAwait(false);

            if (!events.Any())
            {
                return new NullUser();
            }

            user.LoadFromHistory(events);

            return user;
        }
    }
}