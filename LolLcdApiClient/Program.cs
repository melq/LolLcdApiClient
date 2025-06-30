using LolLcdApiClient.Services;
using LolLcdApiClient.Services.Interfaces;
using LolLcdApiClient.Util;
using LolLcdApiClient.Util.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LolLcdApiClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("LoL Companion App");
            Console.WriteLine("==============================");

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ILiveClientDataApiClient, LiveClientDataApiClient>();
                    services.AddSingleton<IJungleTrackerService, JungleTrackerService>();
                    services.AddHostedService<AppHostService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}