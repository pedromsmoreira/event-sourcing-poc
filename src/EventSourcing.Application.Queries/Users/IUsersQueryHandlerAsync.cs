namespace EventSourcing.Application.Queries.Users
{
    using Domain.Users;

    using Infrastructure.Queries;

    public interface IUsersQueryHandlerAsync
        : IQueryHandlerAsync<GetUserByIdQuery, User>
    {
    }
}