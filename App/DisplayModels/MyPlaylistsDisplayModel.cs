namespace SpotifyPlaylistFixer.DisplayModels
{
    using System.Collections.Generic;

    using SpotifyPlaylistFixer.Models;

    public class MyPlaylistsDisplayModel : Pager
    {
        public List<Playlist> Playlists { get; set; }
    }
}