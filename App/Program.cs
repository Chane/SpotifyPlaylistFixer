namespace SpotifyPlaylistFixer;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Handler;
using FluentSpotifyApi.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using SpotifyPlaylistFixer.Services;

[ExcludeFromCodeCoverage]
public static class Program
{
    private const string ClientIdEnvironmentVariable = "SPOTIFY_CLIENT_ID";

    private const string ClientSecretEnvironmentVariable = "SPOTIFY_CLIENT_SECRET";

    private const string SpotifyAuthenticationScheme = SpotifyDefaults.AuthenticationScheme;

    public static void Main(string[] args)
    {
        Console.WriteLine("Process Id: " + Process.GetCurrentProcess().Id);

        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();

        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddSingleton<IPlaylistTrackService, PlaylistTrackService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<IFixedPlaylistService, FixedPlaylistService>();
        services.AddSingleton<IFixProcessor, FixProcessor>();

        ConfigureSpotifyClient(services);

        services.AddControllersWithViews();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSpotifyInvalidRefreshTokenExceptionHandler("/login");

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        ConfigureLoginLogoutRoutes(app);

        app.MapControllers();
    }

    private static void ConfigureLoginLogoutRoutes(WebApplication app)
    {
        app.MapGet(
            "/login",
            async context =>
                {
                    await context.ChallengeAsync(
                        SpotifyAuthenticationScheme,
                        new AuthenticationProperties
                        {
                            RedirectUri = "/",
                            IsPersistent = true
                        });
                });

        app.MapGet(
            "/logout",
            async context =>
                {
                    await context.SignOutAsync();
                    context.Response.Redirect("/");
                });
    }

    private static void ConfigureSpotifyClient(IServiceCollection services)
    {
        var clientId = GetRequiredEnvironmentVariable(ClientIdEnvironmentVariable);
        var clientSecret = GetRequiredEnvironmentVariable(ClientSecretEnvironmentVariable);

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services
            .AddFluentSpotifyClient(clientBuilder => clientBuilder
                .ConfigurePipeline(pipeline => pipeline
                    .AddAspNetCoreAuthorizationCodeFlow(
                        spotifyAuthenticationScheme: SpotifyAuthenticationScheme)));

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                o =>
                    {
                        o.LoginPath = new PathString("/login");
                        o.LogoutPath = new PathString("/logout");
                    })
            .AddSpotify(
                SpotifyAuthenticationScheme,
                o =>
                    {
                        o.ClientId = clientId;
                        o.ClientSecret = clientSecret;
                        o.Scope.Add("playlist-read-private");
                        o.Scope.Add("playlist-read-collaborative");
                        o.Scope.Add("playlist-modify-public");
                        o.Scope.Add("playlist-modify-private");
                        o.SaveTokens = true;
                    });
    }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        throw new InvalidOperationException($"Missing required environment variable: {variableName}");
    }
}