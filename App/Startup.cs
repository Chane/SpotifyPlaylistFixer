namespace SpotifyPlaylistFixer
{
    using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Extensions;
    using FluentSpotifyApi.AuthorizationFlows.AspNetCore.AuthorizationCode.Handler;
    using FluentSpotifyApi.Extensions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using SpotifyPlaylistFixer.Services;

    public class Startup
    {
        private const string ClientId = "..."; // need to get these from spotify
        private const string ClientSecret = "..."; // need to get these from spotify
        private const string SpotifyAuthenticationScheme = SpotifyDefaults.AuthenticationScheme;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPlaylistService, PlaylistService>();
            services.AddSingleton<IPlaylistTrackService, PlaylistTrackService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IFixedPlaylistService, FixedPlaylistService>();
            services.AddSingleton<IFixProcessor, FixProcessor>();
            ConfigureSpotifyClient(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSpotifyInvalidRefreshTokenExceptionHandler("/login");

            app.UseStaticFiles();

            app.UseAuthentication();

            ConfigureRoutes(app);

            app.UseMvc();
        }

        private static void ConfigureRoutes(IApplicationBuilder app)
        {
            app.Map(
                "/login",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            // Return a challenge to invoke the Spotify authentication scheme
                            await context.ChallengeAsync(SpotifyAuthenticationScheme, properties: new AuthenticationProperties { RedirectUri = "/", IsPersistent = true });
                        });
                });

            // Listen for requests on the /logout path, and sign the user out
            app.Map(
                "/logout",
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                            await context.SignOutAsync();

                            // Redirect the user to the home page after signing out
                            context.Response.Redirect("/");
                        });
                });
        }

        private static void ConfigureSpotifyClient(IServiceCollection services)
        {
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
                            o.ClientId = ClientId;
                            o.ClientSecret = ClientSecret;
                            o.Scope.Add("playlist-read-private");
                            o.Scope.Add("playlist-read-collaborative");
                            o.Scope.Add("playlist-modify-public");
                            o.Scope.Add("playlist-modify-private");
                            o.SaveTokens = true;
                        });
        }
    }
}