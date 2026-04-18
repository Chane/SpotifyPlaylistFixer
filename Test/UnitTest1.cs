namespace Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using SpotifyPlaylistFixer.DisplayModels;
    using SpotifyPlaylistFixer.Models;
    using SpotifyPlaylistFixer.Services;

    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task PotentialFixedPlaylist_OnlySearchesLocalTracks()
        {
            var playlistId = "playlist-1";
            var localTrackOne = new Track { Artist = "Artist A", Album = "Album A", TrackName = "Track A", IsLocal = true };
            var remoteTrack = new Track { Artist = "Artist B", Album = "Album B", TrackName = "Track B", IsLocal = false };
            var localTrackTwo = new Track { Artist = "Artist C", Album = "Album C", TrackName = "Track C", IsLocal = true };

            var trackService = new Mock<IPlaylistTrackService>();
            trackService.Setup(s => s.TracksForPlaylist(playlistId)).ReturnsAsync(
                new MyPlaylistTracksDisplayModel
                {
                    PlaylistId = playlistId,
                    PlaylistName = "My Playlist",
                    Tracks = new List<Track> { localTrackOne, remoteTrack, localTrackTwo }
                });

            var fixedTrackOne = new Track { Artist = "Artist A", Album = "Album A", TrackName = "Track A", IsLocal = false, SpotifyUri = "spotify:track:1" };
            var fixedTrackTwo = new Track { Artist = "Artist C", Album = "Album C", TrackName = "Track C", IsLocal = false, SpotifyUri = "spotify:track:2" };
            var searchService = new Mock<ISearchService>();
            searchService.Setup(s => s.Search(localTrackOne.TrackName, localTrackOne.Album, localTrackOne.Artist)).ReturnsAsync(fixedTrackOne);
            searchService.Setup(s => s.Search(localTrackTwo.TrackName, localTrackTwo.Album, localTrackTwo.Artist)).ReturnsAsync(fixedTrackTwo);

            var service = new FixedPlaylistService(trackService.Object, searchService.Object);

            var result = await service.PotentialFixedPlaylist(playlistId);

            Assert.That(result.PlaylistId, Is.EqualTo(playlistId));
            Assert.That(result.PlaylistName, Is.EqualTo("My Playlist"));
            Assert.That(result.Tracks.Count, Is.EqualTo(2));
            Assert.That(result.Tracks[0].SpotifyUri, Is.EqualTo("spotify:track:1"));
            Assert.That(result.Tracks[1].SpotifyUri, Is.EqualTo("spotify:track:2"));

            searchService.Verify(s => s.Search(localTrackOne.TrackName, localTrackOne.Album, localTrackOne.Artist), Times.Once);
            searchService.Verify(s => s.Search(localTrackTwo.TrackName, localTrackTwo.Album, localTrackTwo.Artist), Times.Once);
            searchService.Verify(s => s.Search(remoteTrack.TrackName, remoteTrack.Album, remoteTrack.Artist), Times.Never);
        }

        [Test]
        public async Task PotentialFixedPlaylist_WithNoLocalTracks_ReturnsEmptySuggestedTracks()
        {
            var playlistId = "playlist-2";
            var trackService = new Mock<IPlaylistTrackService>();
            trackService.Setup(s => s.TracksForPlaylist(playlistId)).ReturnsAsync(
                new MyPlaylistTracksDisplayModel
                {
                    PlaylistId = playlistId,
                    PlaylistName = "Remote Only",
                    Tracks = new List<Track>
                    {
                        new Track { Artist = "Artist", Album = "Album", TrackName = "Track", IsLocal = false }
                    }
                });

            var searchService = new Mock<ISearchService>();
            var service = new FixedPlaylistService(trackService.Object, searchService.Object);

            var result = await service.PotentialFixedPlaylist(playlistId);

            Assert.That(result.PlaylistId, Is.EqualTo(playlistId));
            Assert.That(result.PlaylistName, Is.EqualTo("Remote Only"));
            Assert.That(result.Tracks.Count, Is.EqualTo(0));
            searchService.Verify(
                s => s.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

    }
}