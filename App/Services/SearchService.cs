namespace SpotifyPlaylistFixer.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentSpotifyApi;
    using FluentSpotifyApi.Model.Messages;

    using SpotifyPlaylistFixer.Models;

    public class SearchService : ISearchService
    {
        private readonly IFluentSpotifyClient fluentSpotifyClient;

        public SearchService(IFluentSpotifyClient fluentSpotifyClient)
        {
            this.fluentSpotifyClient = fluentSpotifyClient;
        }

        public Task<FullTracksPageMessage> GetAsync(string track, string album, string artist)
        {
            var query = $"track:{track} album:{album} artist:{artist}"; 
            var result = this.fluentSpotifyClient.Search.Tracks.Matching(query).GetAsync(limit: 1);
            return result;
        }

        public async Task<Track> Search(string track, string album, string artist)
        {
            var query = $"track:{track} album:{album} artist:{artist}".Replace("\'", string.Empty);
            var result = await this.fluentSpotifyClient.Search.Tracks.Matching(query).GetAsync(limit: 1);
            var spotifyTrack = result.Page.Items.FirstOrDefault();
            Track model;
            if (spotifyTrack != null)
            {
                model = new Track
                            {
                                Artist = spotifyTrack.Artists.First().Name,
                                Album = spotifyTrack.Album.Name,
                                TrackName = spotifyTrack.Name,
                                IsLocal = spotifyTrack.Uri.Contains("local", StringComparison.OrdinalIgnoreCase),
                                SpotifyUri = spotifyTrack.Uri
                            };
            }
            else
            {
                model = await this.Search(track, artist);
                if (string.IsNullOrWhiteSpace(model.Album))
                {
                    model.Album = $"Not found on: {album}";
                }
            }

            return model;
        }

        private async Task<Track> Search(string track, string artist)
        {
            var query = $"track:{track} artist:{artist}".Replace("\'", string.Empty);
            var result = await this.fluentSpotifyClient.Search.Tracks.Matching(query).GetAsync(limit: 1);
            var spotifyTrack = result.Page.Items.FirstOrDefault();
            Track model;
            if (spotifyTrack != null)
            {
                model = new Track
                            {
                                Artist = spotifyTrack.Artists.First().Name,
                                Album = spotifyTrack.Album.Name,
                                TrackName = spotifyTrack.Name,
                                IsLocal = spotifyTrack.Uri.Contains("local", StringComparison.OrdinalIgnoreCase),
                                SpotifyUri = spotifyTrack.Uri
                            };
            }
            else
            {
                model = new Track
                            {
                                Artist = artist,
                                TrackName = track,
                                IsLocal = true,
                                SpotifyUri = "Not Found"
                            };
            }

            return model;
        }
    }
}