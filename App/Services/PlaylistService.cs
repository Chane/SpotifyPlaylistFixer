namespace SpotifyPlaylistFixer.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using FluentSpotifyApi;
    using FluentSpotifyApi.Model;

    using SpotifyPlaylistFixer.DisplayModels;
    using SpotifyPlaylistFixer.Models;

    public class PlaylistService : IPlaylistService
    {
        private const int DefaultPageSize = 20;
        private readonly IFluentSpotifyClient fluentSpotifyClient;

        public PlaylistService(IFluentSpotifyClient fluentSpotifyClient)
        {
            this.fluentSpotifyClient = fluentSpotifyClient;
        }

        public async Task<MyPlaylistsDisplayModel> GetPlaylists(int? page)
        {
            var result = await this.GetAsync(page).ConfigureAwait(false);
            var playlists = result.Items.Select(
                i => new Playlist
                         {
                             PlaylistId = i.Id,
                             PlaylistName = i.Name
                         });
            var displayModel = new MyPlaylistsDisplayModel
                                   {
                                       Playlists = playlists.ToList(),
                                       PageNumber = page ?? 1,
                                       PageSize = DefaultPageSize,
                                       TotalRecords = result.Total
            };
            return displayModel;
        }

        public Task<Page<SimplePlaylist>> GetAsync(int? page)
        {
            int offset = CalculateOffset(page);
            return this.fluentSpotifyClient.Me.Playlists.GetAsync(limit: DefaultPageSize, offset: offset);
        }

        private static int CalculateOffset(int? page)
        {
            var offset = 0;
            if (page.HasValue)
            {
                offset = (page.Value * DefaultPageSize) - DefaultPageSize;
            }

            return offset;
        }
    }
}