namespace SpotifyPlaylistFixer.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentSpotifyApi;
    using FluentSpotifyApi.Model;

    using SpotifyPlaylistFixer.DisplayModels;
    using SpotifyPlaylistFixer.Models;

    public class PlaylistTrackService : IPlaylistTrackService
    {
        private readonly IFluentSpotifyClient fluentSpotifyClient;

        public PlaylistTrackService(IFluentSpotifyClient fluentSpotifyClient)
        {
            this.fluentSpotifyClient = fluentSpotifyClient;
        }

        public async Task<MyPlaylistTracksDisplayModel> TracksForPlaylist(string playlistId)
        {
            var result = await this.GetAsync(playlistId).ConfigureAwait(false);
            var tracks = result.Tracks.Items.Select(
                i => new Track
                         {
                             Artist = i.Track.Artists[0].Name,
                             Album = i.Track.Album.Name,
                             TrackName = i.Track.Name,
                             IsLocal = i.Track.Uri.Contains("local", StringComparison.OrdinalIgnoreCase),
                             SpotifyUri = i.Track.Uri
                         });
            var displayModel = new MyPlaylistTracksDisplayModel
                                   {
                                       PlaylistName = result.Name,
                                       Tracks = tracks.ToList(),
                                       PlaylistId = playlistId
                                   };
            return displayModel;
        }

        public Task<FullPlaylist> GetAsync(string playlistId)
        {
            var result = this.fluentSpotifyClient.Me.Playlist(playlistId).GetAsync();
            return result;
        }
    }
}