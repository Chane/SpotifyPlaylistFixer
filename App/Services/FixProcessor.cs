namespace SpotifyPlaylistFixer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentSpotifyApi;

    using SpotifyPlaylistFixer.Models;

    public class FixProcessor : IFixProcessor
    {
        private readonly IFluentSpotifyClient fluentSpotifyClient;

        public FixProcessor(IFluentSpotifyClient fluentSpotifyClient)
        {
            this.fluentSpotifyClient = fluentSpotifyClient;
        }

        public async Task DoFix(string playlistId, List<Track> newTracks)
        {
            var ids = newTracks.Where(t => !string.Equals(t.SpotifyUri, "Not Found", StringComparison.OrdinalIgnoreCase))
                .Select(t => t.SpotifyUri.Replace("spotify:track:", string.Empty));
            var result = await this.fluentSpotifyClient.Me.Playlist(playlistId)
                .Tracks(ids)
                .AddAsync();
            Console.WriteLine(result);
        }
    }
}