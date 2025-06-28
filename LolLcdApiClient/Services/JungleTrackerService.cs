using LolLcdApiClient.Models;
using LolLcdApiClient.Services.Interfaces;
using LolLcdApiClient.Util;
using LolLcdApiClient.Util.Interfaces;
using System.Text.Json;

namespace LolLcdApiClient.Services
{
    /// <summary>
    /// <inheritdoc cref="IJungleTrackerService"/>"/>
    /// </summary>
    /// <param name="liveClientDataApiClient"></param>
    public class JungleTrackerService(ILiveClientDataApiClient liveClientDataApiClient) : IJungleTrackerService
    {
        private readonly string getPlayerListEndpoint = Const.ApiUrls.playerListApiUrl;
        private readonly string getActivePlayerEndpoint = Const.ApiUrls.activePlayerApiUrl;
        private PlayerData? _lastKnownEnemyJungler;

        /// <inheritdoc/>
        public async Task StartTrackingAsync()
        {
            ConsoleWriter.PrintLine("ゲームが開始されると、1秒ごとに相手JGの情報を監視します。");

            while (true)
            {
                try
                {
                    var allPlayers = await GetPlayerListAsync();
                    var activePlayer = await GetActivePlayerAsync();

                    if (allPlayers == null || activePlayer == null) continue;

                    var me = allPlayers.FirstOrDefault(p => p.SummonerName == activePlayer.SummonerName);
                    if (me == null) continue;

                    var currentEnemyJungler = allPlayers.FirstOrDefault(p => p.Team != me.Team && p.Position == "JUNGLE");

                    if (currentEnemyJungler == null)
                    {
                        if (_lastKnownEnemyJungler != null)
                        {
                            ConsoleWriter.PrintLine("相手JGが見つかりません。");
                            _lastKnownEnemyJungler = null;
                        }
                        continue;
                    }

                    if (_lastKnownEnemyJungler == null)
                    {
                        ConsoleWriter.PrintLine($"相手JGを検出: {currentEnemyJungler.ChampionName} ({currentEnemyJungler.SummonerName})", ConsoleColor.White);
                    }
                    else
                    {
                        CompareAndReportChanges(_lastKnownEnemyJungler, currentEnemyJungler);
                    }

                    _lastKnownEnemyJungler = currentEnemyJungler;
                }
                catch (HttpRequestException)
                {
                    if (_lastKnownEnemyJungler != null)
                    {
                        ConsoleWriter.PrintWarningLine("ゲームが終了したか、接続が切れました。待機状態に戻ります。");
                        _lastKnownEnemyJungler = null;
                    }
                    ConsoleWriter.Print(".");
                }
                catch (Exception ex)
                {
                    ConsoleWriter.PrintErrorLine($"\n予期せぬエラーが発生しました。: {ex.Message}");
                }

                await Task.Delay(500);
            }
        }

        private void CompareAndReportChanges(PlayerData lastState, PlayerData currentState)
        {
            // レベルの変化
            if (currentState.Level > lastState.Level)
                ConsoleWriter.PrintLine($"JG Level Up: {lastState.Level} -> {currentState.Level}", ConsoleColor.Green);

            // KDAの変化
            /*if (currentState.Scores.Kills > lastState.Scores.Kills)
                ConsoleWriter.PrintLine($"JG Kills: {lastState.Scores.Kills} -> {currentState.Scores.Kills}", ConsoleColor.Cyan);
            if (currentState.Scores.Deaths > lastState.Scores.Deaths)
                ConsoleWriter.PrintLine($"JG Deaths: {lastState.Scores.Deaths} -> {currentState.Scores.Deaths}", ConsoleColor.Red);
            if (currentState.Scores.Assists > lastState.Scores.Assists)
                ConsoleWriter.PrintLine($"JG Assists: {lastState.Scores.Assists} -> {currentState.Scores.Assists}", ConsoleColor.Blue);*/
            if (currentState.Scores.Assists > lastState.Scores.CreepScore)
                ConsoleWriter.PrintLine($"JG Assists: {lastState.Scores.CreepScore} -> {currentState.Scores.CreepScore}", ConsoleColor.Blue);

            // アイテムの変化
            var lastItemNames = new HashSet<string>(lastState.Items.Select(i => i.ItemName));
            var currentItemNames = new HashSet<string>(currentState.Items.Select(i => i.ItemName));

            if (!currentItemNames.SetEquals(lastItemNames))
            {
                var newItems = currentItemNames.Except(lastItemNames);
                foreach (var item in newItems)
                {
                    ConsoleWriter.PrintLine($"JG New Item: {item}", ConsoleColor.Yellow);
                }
            }
        }

        private async Task<List<PlayerData>> GetPlayerListAsync()
        {
            var json = await liveClientDataApiClient.GetStringAsync(getPlayerListEndpoint);
            return JsonSerializer.Deserialize<List<PlayerData>>(json) ?? [];
        }

        private async Task<ActivePlayer> GetActivePlayerAsync()
        {
            var json = await liveClientDataApiClient.GetStringAsync(getActivePlayerEndpoint);
            return JsonSerializer.Deserialize<ActivePlayer>(json) ?? new ActivePlayer();
        }
    }
}
