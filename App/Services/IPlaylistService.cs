namespace SpotifyPlaylistFixer.Services
{
    using System.Threading.Tasks;

    using FluentSpotifyApi.Model;

    using SpotifyPlaylistFixer.DisplayModels;

    public interface IPlaylistService
    {
        Task<Page<SimplePlaylist>> GetAsync(int? page);

        Task<MyPlaylistsDisplayModel> GetPlaylists(int? page);
    }
}