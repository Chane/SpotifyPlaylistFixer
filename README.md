# SpotifyPlaylistFixer

SpotifyPlaylistFixer is an ASP.NET Core MVC app that helps you repair Spotify playlists that contain local tracks by searching Spotify for equivalents and adding matches back into the playlist.

## What The App Does

- Authenticates with Spotify using OAuth via FluentSpotifyApi.
- Lists your playlists.
- Shows tracks for a selected playlist.
- Detects local tracks in that playlist.
- Searches Spotify for each local track and proposes replacements.
- Lets you apply the proposed replacements back to the same playlist.

## Current Tech Stack

- .NET target: `net8.0`.
- Web framework: ASP.NET Core MVC with minimal hosting.
- Spotify integration: FluentSpotifyApi 2.x.
- Tests: NUnit + Moq.

## Setup

### 1. Create Spotify app credentials

Create an app in the Spotify Developer Dashboard and obtain:

- Client ID
- Client Secret

### 2. Configure environment variables

The app reads credentials from environment variables:

- `SPOTIFY_CLIENT_ID`
- `SPOTIFY_CLIENT_SECRET`

Example on Linux/macOS:

```bash
export SPOTIFY_CLIENT_ID="your-client-id"
export SPOTIFY_CLIENT_SECRET="your-client-secret"
```

### 3. Install .NET 8 SDK

Use .NET SDK `8.0.x` (the repo includes `global.json` to pin this major version).

### 4. Restore and run

```bash
dotnet restore SpotifyPlaylistFixer.sln
dotnet run --project App/SpotifyPlaylistFixer.csproj
```

## Routes You Will Use

- `/Playlists/display`: Shows your playlists.
- `/Playlist/Display/{id}`: Shows tracks for a playlist.
- `/Comparison/{id}`: Shows original vs suggested replacement tracks.
- `/Playlist/Display/{id}/CheckFix`: Preview of suggested fixed tracks.
- `/Playlist/Display/{id}/DoFix`: Applies suggested replacements.

## Testing

Run tests with:

```bash
dotnet test SpotifyPlaylistFixer.sln
```

Current unit tests focus on:

- Local-track filtering and replacement behavior in `FixedPlaylistService`.

## CI

GitHub Actions workflow is defined in `.github/workflows/ci.yml` and runs:

- `dotnet restore`
- `dotnet build`
- `dotnet test`

on push to `main` and on pull requests.

## Security Notes

- Do not commit real Spotify credentials.
- This repository now expects credentials from environment variables instead of hardcoded values in source.
- Generated `bin/` and `obj/` output can contain local machine paths and should remain untracked.

## Known Limitations

- Spotify API ecosystem upgrades can introduce authentication-flow behavior changes between major package versions.
- If FluentSpotifyApi runtime behavior changes unexpectedly, fallback migration target is `net6.0` LTS with the same hosting model.