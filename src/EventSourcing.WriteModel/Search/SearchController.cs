namespace EventSourcing.WriteModel.Search
{
    using System.Threading.Tasks;

    using Application.Queries.Search;

    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    public class SearchController : Controller
    {
        private readonly ISearchQueryHandlerAsync searchQueryHandler;

        public SearchController(ISearchQueryHandlerAsync searchQueryHandler)
        {
            this.searchQueryHandler = searchQueryHandler;
        }

        [Route("users/search")]
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string name = "",
            [FromQuery] string job = "", 
            [FromQuery] int page = 1,
            [FromQuery] int limit = 60)
        {
            // move parameters to object
            var searchQuery = new SearchUsersQuery
            {
                Name = name,
                Job = job,
                Limit = limit,
                Page = page
            };

            var searhResults = await this.searchQueryHandler.HandleAsync(searchQuery).ConfigureAwait(false);

            return this.Ok(searhResults);
        }
    }
}