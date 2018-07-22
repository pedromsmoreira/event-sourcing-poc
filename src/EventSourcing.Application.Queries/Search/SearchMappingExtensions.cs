namespace EventSourcing.Application.Queries.Search
{
    using Dto.Users;

    using Domain = Domain.Users.Search;

    public static class SearchMappingExtensions
    {
        public static UserSearchResponse MapTo(this Domain.UserSearchResponse response)
        {
            if (response == null)
            {
                return new UserSearchResponse();
            }

            return new UserSearchResponse
            {
                Name = response.Name,
                Job = response.Job,
                Id = response.Id,
                LastSnapshot = response.LastSnapshot
            };
        }
    }
}