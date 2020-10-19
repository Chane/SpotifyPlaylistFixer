namespace SpotifyPlaylistFixer.DisplayModels
{
    public class ComparisonDisplayModel
    {
        public MyPlaylistTracksDisplayModel OriginalTracks { get; set; }

        public MyPlaylistTracksDisplayModel SuggestedTracks { get; set; }

        public string PlaylistName { get; set; }

        public string PlaylistId { get; set; }
    }
}