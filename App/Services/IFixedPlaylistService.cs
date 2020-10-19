namespace SpotifyPlaylistFixer.Services
{
    using System.Threading.Tasks;

    using SpotifyPlaylistFixer.DisplayModels;

    public interface IFixedPlaylistService
    {
        Task<MyPlaylistTracksDisplayModel> PotentialFixedPlaylist(string playlistId);
    }
}