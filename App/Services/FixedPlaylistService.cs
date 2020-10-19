namespace SpotifyPlaylistFixer.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SpotifyPlaylistFixer.DisplayModels;
    using SpotifyPlaylistFixer.Models;

    public class FixedPlaylistService : IFixedPlaylistService
    {
        private readonly IPlaylistTrackService trackService;

        private readonly ISearchService searchService;

        public FixedPlaylistService(IPlaylistTrackService trackService, ISearchService searchService)
        {
            this.trackService = trackService;
            this.searchService = searchService;
        }

        public async Task<MyPlaylistTracksDisplayModel> PotentialFixedPlaylist(string playlistId)
        {
            var playlistTracks = await this.trackService.TracksForPlaylist(playlistId);
            var fixedTracks = new List<Track>();
            foreach (var track in playlistTracks.Tracks)
            {
                if (track.IsLocal)
                {
                    var fixedTrack = await this.searchService.Search(track.TrackName, track.Album, track.Artist);
                    fixedTracks.Add(fixedTrack);
                }
            }

            var model = new MyPlaylistTracksDisplayModel
                            {
                                Tracks = fixedTracks,
                                PlaylistName = playlistTracks.PlaylistName,
                                PlaylistId = playlistId
                            };

            return model;
        }
    }
}