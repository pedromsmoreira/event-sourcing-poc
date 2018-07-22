namespace EventSourcing.Persistence.ElasticSearch.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Model;

    using Domain.Users.Search;

    using Nest;

    public class UsersSearchRepository
    {
        private const string IndexName = "users";
        private const string TypeName = "userindex";
        private readonly IElasticClient client;

        public UsersSearchRepository(IElasticClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<UserSearchResponse>> SearchUserWithFiltersAsync(SearchUsersFilters filters)
        {
            SearchDescriptor<UserIndex> Search(SearchDescriptor<UserIndex> s) => s.From((filters.Page - 1) * filters.Limit)
                .Size(filters.Limit)
                .Index(IndexName)
                .Type(TypeName);

            var searchQuery = BuildSearchUsersQuery(filters);

            var searchResponse = await this.client
                .SearchAsync<UserIndex>(
                    s => Search(s)
                        .Query(searchQuery))
                .ConfigureAwait(false);

            return searchResponse.Documents.Select(
                d =>
                    new UserSearchResponse
                    {
                        Name = d.Name,
                        Job = d.Job,
                        Id = d.Id,
                        LastSnapshot = d.LastSnapshot
                    });
        }

        private static Func<QueryContainerDescriptor<UserIndex>, QueryContainer> BuildSearchUsersQuery(SearchUsersFilters filters)
        {
            var nameFilter = new QueryContainerDescriptor<UserIndex>()
                .Match(m => m
                    .Field(f => f.Name)
                    .Analyzer("standard")
                    .Fuzziness(Fuzziness.Auto)
                    .Lenient()
                    .FuzzyTranspositions()
                    .Query(filters.Name));

            var jobFilter = new QueryContainerDescriptor<UserIndex>()
                .Match(m => m
                    .Field(f => f.Job)
                    .Analyzer("standard")
                    .Fuzziness(Fuzziness.Auto)
                    .Lenient()
                    .FuzzyTranspositions()
                    .Query(filters.Job));

            Func<QueryContainerDescriptor<UserIndex>, QueryContainer> searchQuery = sq => sq;

            if (!string.IsNullOrEmpty(filters.Name) && !string.IsNullOrEmpty(filters.Job))
            {
                searchQuery = sq => +nameFilter && jobFilter;
            }
            else
            {
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    searchQuery = sq => +nameFilter;
                }
                else
                {
                    if (!string.IsNullOrEmpty(filters.Job))
                    {
                        searchQuery = sq => +jobFilter;
                    }
                }
            }

            return searchQuery;
        }
    }
}