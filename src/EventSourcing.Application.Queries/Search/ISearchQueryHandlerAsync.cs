namespace EventSourcing.Application.Queries.Search
{
    using System.Collections.Generic;

    using Dto.Users;

    using Infrastructure.Queries;

    public interface ISearchQueryHandlerAsync
        : IQueryHandlerAsync<SearchUsersQuery, IEnumerable<UserSearchResponse>>
    {
    }
}