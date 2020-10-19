namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.Services;

    public class CheckPlaylistFixController : Controller
    {
        private readonly IFixedPlaylistService service;

        public CheckPlaylistFixController(IFixedPlaylistService service)
        {
            this.service = service;
        }
        
        [Authorize]
        [HttpGet("Playlist/Display/{id}/CheckFix")]
        public async Task<IActionResult> Display(string id)
        {
            var displayModel = await this.service.PotentialFixedPlaylist(id);

            return this.View("Tracks", displayModel);
        }
    }
}