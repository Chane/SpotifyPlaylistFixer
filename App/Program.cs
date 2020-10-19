namespace SpotifyPlaylistFixer
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Process Id: " + Process.GetCurrentProcess().Id);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}