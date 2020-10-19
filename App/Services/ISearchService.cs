namespace SpotifyPlaylistFixer.Services
{
    using System.Threading.Tasks;

    using FluentSpotifyApi.Model.Messages;

    using SpotifyPlaylistFixer.Models;

    public interface ISearchService
    {
        Task<FullTracksPageMessage> GetAsync(string track, string album, string artist);

        Task<Track> Search(string track, string album, string artist);
    }
}