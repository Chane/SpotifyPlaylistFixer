namespace SpotifyPlaylistFixer.Services
{
    using System.Threading.Tasks;

    using FluentSpotifyApi.Model;

    using SpotifyPlaylistFixer.DisplayModels;

    public interface IPlaylistTrackService
    {
        Task<FullPlaylist> GetAsync(string playlistId);

        Task<MyPlaylistTracksDisplayModel> TracksForPlaylist(string playlistId);
    }
}