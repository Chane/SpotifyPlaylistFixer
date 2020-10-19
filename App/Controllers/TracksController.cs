namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.Services;

    public class TracksController : Controller
    {
        private readonly IPlaylistTrackService service;

        public TracksController(IPlaylistTrackService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet("Tracks/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await this.service.GetAsync(id);

            return this.Json(result);
        }

        [Authorize]
        [HttpGet("Playlist/Display/{id}")]
        public async Task<IActionResult> Display(string id)
        {
            var displayModel = await this.service.TracksForPlaylist(id);

            return this.View("Tracks", displayModel);
        }
    }
}