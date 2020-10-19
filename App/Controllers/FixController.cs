namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.Services;

    public class FixController : Controller
    {
        private readonly IFixProcessor processor;

        private readonly IFixedPlaylistService service;

        public FixController(IFixProcessor processor, IFixedPlaylistService service)
        {
            this.processor = processor;
            this.service = service;
        }

        [Authorize]
        [HttpGet("Playlist/Display/{id}/DoFix")]
        public async Task<IActionResult> DoFix(string id)
        {
            var result = await this.service.PotentialFixedPlaylist(id);
            await this.processor.DoFix(id, result.Tracks);
            return this.Redirect("/Playlist/Display/" + id);
        }
    }
}