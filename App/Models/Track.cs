namespace SpotifyPlaylistFixer.Models
{
    public class Track
    {
        public string Artist { get; set; }

        public string Album { get; set; }

        public string TrackName { get; set; }

        public bool IsLocal { get; set; }

        public string SpotifyUri { get; set; }
    }
}