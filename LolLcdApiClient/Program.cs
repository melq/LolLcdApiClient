// Program.cs
using LolLcdApiClient.Services;
using LolLcdApiClient.Util;

namespace LolLcdApiClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Enemy Jungler Tracker)");
            Console.WriteLine("=====================================");

            var apiService = new LiveClientDataApiClient();

            var trackerService = new JungleTrackerService(apiService);

            await trackerService.StartTrackingAsync();
        }
    }
}