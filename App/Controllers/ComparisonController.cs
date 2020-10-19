namespace SpotifyPlaylistFixer.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SpotifyPlaylistFixer.DisplayModels;
    using SpotifyPlaylistFixer.Services;

    public class ComparisonController : Controller
    {
        private readonly IPlaylistTrackService trackService;

        private readonly IFixedPlaylistService fixedPlaylistService;

        public ComparisonController(IPlaylistTrackService trackService, IFixedPlaylistService fixedPlaylistService)
        {
            this.trackService = trackService;
            this.fixedPlaylistService = fixedPlaylistService;
        }

        [Authorize]
        [HttpGet("[Controller]/{id}")]
        public async Task<IActionResult> Display(string id)
        {
            var playlistTask = this.trackService.TracksForPlaylist(id);
            var fixedTrackTask = this.fixedPlaylistService.PotentialFixedPlaylist(id);

            await Task.WhenAll(playlistTask, fixedTrackTask);

            var originalPlaylist = playlistTask.Result;
            var displayModel = new ComparisonDisplayModel();
            displayModel.OriginalTracks = originalPlaylist;
            displayModel.SuggestedTracks = fixedTrackTask.Result;
            displayModel.PlaylistName = originalPlaylist.PlaylistName;
            displayModel.PlaylistId = originalPlaylist.PlaylistId;

            return this.View("Index", displayModel);
        }
    }
}