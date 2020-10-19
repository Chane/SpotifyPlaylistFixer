namespace SpotifyPlaylistFixer.Models
{
    public class Pager
    {
        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages => (this.TotalRecords / this.PageSize ?? 1) + 1;
    }
}