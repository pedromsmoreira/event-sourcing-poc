namespace EventSourcing.Application.Queries.Search
{
    using Infrastructure.Queries;

    public class SearchUsersQuery : IQuery
    {
        public string Name { get; set; }

        public string Job { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }
    }
}