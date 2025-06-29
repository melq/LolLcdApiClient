using LolLcdApiClient.Services.Interfaces;
using LolLcdApiClient.Util;
using Microsoft.Extensions.Hosting;

namespace LolLcdApiClient.Services
{
    /// <summary>
    /// <inheritdoc cref="IHostedService"/>/>
    /// </summary>
    /// <param name="jungleTracker"><see cref="IJungleTrackerService"/></param>
    /// <param name="teamFightMonitor"><see cref="ITeamFightMonitorService"/></param>
    public class AppHostService(IJungleTrackerService jungleTracker, ITeamFightMonitorService teamFightMonitor) : IHostedService
    {
        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ConsoleWriter.PrintLine("ホストサービスを開始します。各サービスを起動...", ConsoleColor.Green);

            _ = jungleTracker.StartTrackingAsync();
            _ = teamFightMonitor.StartMonitoringAsync();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            ConsoleWriter.PrintLine("ホストサービスを停止します。", ConsoleColor.Yellow);
            return Task.CompletedTask;
        }
    }
}