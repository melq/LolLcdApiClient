using LolLcdApiClient.Models;
using LolLcdApiClient.Services.Interfaces;
using LolLcdApiClient.Util;
using LolLcdApiClient.Util.Interfaces;

namespace LolLcdApiClient.Services
{
    /// <summary>
    /// <inheritdoc cref="IJungleTrackerService"/>
    /// </summary>
    /// <param name="liveClientDataApiClient"><see cref="ILiveClientDataApiClient"/></param>
    public class JungleTrackerService(ILiveClientDataApiClient liveClientDataApiClient) : IJungleTrackerService
    {
        private PlayerData? _lastKnownEnemyJungler;

        /// <inheritdoc/>
        public async Task StartTrackingAsync()
        {
            ConsoleWriter.PrintLine("相手JGの情報の監視を開始します。");

            while (true)
            {
                try
                {
                    var allPlayers = await liveClientDataApiClient.GetPlayerListAsync();
                    var activePlayer = await liveClientDataApiClient.GetActivePlayerAsync();

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
                    CompareAndReportChanges(currentEnemyJungler);

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
                    ConsoleWriter.PrintErrorLine($"\n[JungleTracker] エラー: {ex.Message}");
                }

                await Task.Delay(500);
            }
        }

        /// <summary>
        /// 敵ジャングラーの状態を前回の状態と比較し、変更があれば出力します。
        /// </summary>
        /// <param name="currentEnemyJungler">現在の敵ジャングラー</param>
        private void CompareAndReportChanges(PlayerData currentEnemyJungler)
        {
            if (_lastKnownEnemyJungler == null)
            {
                ConsoleWriter.PrintLine($"相手JGを検出: {currentEnemyJungler.ChampionName} ({currentEnemyJungler.SummonerName})", ConsoleColor.White);
                return;
            }

            if (currentEnemyJungler.Level > _lastKnownEnemyJungler.Level)
                ConsoleWriter.PrintLine($"JG Level Up: {_lastKnownEnemyJungler.Level} -> {currentEnemyJungler.Level}", ConsoleColor.Green);

            /*if (currentState.Scores.Kills > lastState.Scores.Kills)
                ConsoleWriter.PrintLine($"JG Kills: {lastState.Scores.Kills} -> {currentState.Scores.Kills}", ConsoleColor.Cyan);
            if (currentState.Scores.Deaths > lastState.Scores.Deaths)
                ConsoleWriter.PrintLine($"JG Deaths: {lastState.Scores.Deaths} -> {currentState.Scores.Deaths}", ConsoleColor.Red);
            if (currentState.Scores.Assists > lastState.Scores.Assists)
                ConsoleWriter.PrintLine($"JG Assists: {lastState.Scores.Assists} -> {currentState.Scores.Assists}", ConsoleColor.Blue);*/
            if (currentEnemyJungler.Scores.Assists > _lastKnownEnemyJungler.Scores.CreepScore)
                ConsoleWriter.PrintLine($"JG Assists: {_lastKnownEnemyJungler.Scores.CreepScore} -> {currentEnemyJungler.Scores.CreepScore}", ConsoleColor.Blue);

            var lastItemNames = new HashSet<string>(_lastKnownEnemyJungler.Items.Select(i => i.ItemName));
            var currentItemNames = new HashSet<string>(currentEnemyJungler.Items.Select(i => i.ItemName));

            if (!currentItemNames.SetEquals(lastItemNames))
            {
                var newItems = currentItemNames.Except(lastItemNames);
                foreach (var item in newItems)
                {
                    ConsoleWriter.PrintLine($"JG New Item: {item}", ConsoleColor.Yellow);
                }
            }
        }
    }
}
