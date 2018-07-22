namespace EventSourcing.Application.Queries.Search
{
    using Domain.Users.Search;

    // Improve this into fluent builder maybe or at least guarantee some order when building filters
    public class SearchUsersFilterBuilder
    {
        private SearchUsersFilters searchFilters;

        public SearchUsersFilterBuilder Create(int page, int limit)
        {
            this.searchFilters = new SearchUsersFilters(page, limit);
            return this;
        }

        public SearchUsersFilterBuilder AndName(string name)
        {
            this.searchFilters.Name = name;
            return this;
        }

        public SearchUsersFilterBuilder AndJob(string job)
        {
            this.searchFilters.Job = job;
            return this;
        }

        public SearchUsersFilters Build()
        {
            return this.searchFilters;
        }
    }
}