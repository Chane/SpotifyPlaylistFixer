namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.Services;

    public class SearchController : Controller
    {
        private readonly ISearchService service;

        public SearchController(ISearchService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet("/SampleSearch")]
        public async Task<IActionResult> SampleSearch()
        {
            var result = await this.service.Search("Hells Bells", "Back in Black", "AC/DC");
            return this.Json(result);
        }
    }
}