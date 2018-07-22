namespace EventSourcing.Application.Queries.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Persistence.ElasticSearch.Users;

    using UserSearchResponse = Dto.Users.UserSearchResponse;

    public class SearchQueryHandlerAsync : ISearchQueryHandlerAsync
    {
        private readonly UsersSearchRepository searchRepository;

        public SearchQueryHandlerAsync(UsersSearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
        }

        public async Task<IEnumerable<UserSearchResponse>> HandleAsync(SearchUsersQuery query)
        {
            var searchFilters = new SearchUsersFilterBuilder()
                .Create(query.Page, query.Limit)
                .AndName(query.Name)
                .AndJob(query.Job)
                .Build();

            var response = (await this.searchRepository.SearchUserWithFiltersAsync(searchFilters).ConfigureAwait(false)).ToList();

            if (!response.Any())
            {
                return new List<UserSearchResponse>();
            }

            return response.Select(ur => ur.MapTo());
        }
    }
}