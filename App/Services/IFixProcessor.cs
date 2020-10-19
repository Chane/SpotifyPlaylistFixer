using System.Collections.Generic;
using SpotifyPlaylistFixer.Models;

namespace SpotifyPlaylistFixer.Services
{
    using System.Threading.Tasks;

    public interface IFixProcessor
    {
        Task DoFix(string playlistId, List<Track> newTracks);
    }
}