namespace SpotifyPlaylistFixer.DisplayModels
{
    using System.Collections.Generic;

    using SpotifyPlaylistFixer.Models;

    public class MyPlaylistTracksDisplayModel
    {
        public string PlaylistName { get; set; }

        public string PlaylistId { get; set; }

        public List<Track> Tracks { get; set; }
    }
}