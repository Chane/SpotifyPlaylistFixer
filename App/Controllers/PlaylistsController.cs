namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.Services;

    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService service;
        
        public PlaylistsController(IPlaylistService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet("/myplaylists")]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int? page)
        {
            var result = await this.service.GetAsync(page);
            return this.Json(result);
        }

        [Authorize]
        [HttpGet("[Controller]/display")]
        public async Task<IActionResult> Display([FromQuery(Name = "page")] int? page)
        {
            var displayModel = await this.service.GetPlaylists(page);
            return this.View("Playlists", displayModel);
        }
    }
}