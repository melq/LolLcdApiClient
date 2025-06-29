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

        private static readonly List<(ConsoleColor Background, ConsoleColor Foreground)> _attentionColors =
        [
            (ConsoleColor.DarkRed, ConsoleColor.White),
            (ConsoleColor.DarkBlue, ConsoleColor.Yellow),
            (ConsoleColor.DarkMagenta, ConsoleColor.Cyan),
            (ConsoleColor.DarkGreen, ConsoleColor.White),
            (ConsoleColor.DarkYellow, ConsoleColor.Black)
        ];

        private static int _colorIndex = 0;

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

            var changesToReport = new List<Action>();

            if (currentEnemyJungler.Level > _lastKnownEnemyJungler.Level)
            {
                changesToReport.Add(() =>
                    ConsoleWriter.PrintLine($"JG Level Up: {_lastKnownEnemyJungler.Level} -> {currentEnemyJungler.Level}", ConsoleColor.Green)
                );
            }

            if (currentEnemyJungler.Scores.CreepScore > _lastKnownEnemyJungler.Scores.CreepScore)
            {
                changesToReport.Add(() =>
                    ConsoleWriter.PrintLine($"JG CS: {_lastKnownEnemyJungler.Scores.CreepScore} -> {currentEnemyJungler.Scores.CreepScore}", ConsoleColor.Cyan)
                );
            }

            var lastItemNames = new HashSet<string>(_lastKnownEnemyJungler.Items.Select(i => i.ItemName));
            var currentItemNames = new HashSet<string>(currentEnemyJungler.Items.Select(i => i.ItemName));

            if (!currentItemNames.SetEquals(lastItemNames))
            {
                var newItems = currentItemNames.Except(lastItemNames);
                foreach (var item in newItems)
                {
                    changesToReport.Add(() =>
                        ConsoleWriter.PrintLine($"JG New Item: {item}", ConsoleColor.Yellow)
                    );
                }
            }

            if (changesToReport.Count != 0)
            {
                var selectedColor = _attentionColors[_colorIndex];
                _colorIndex = (_colorIndex + 1) % _attentionColors.Count;
                ConsoleWriter.PrintAttentionHeader("Enemy Jungler Status Updated", selectedColor);

                foreach (var reportAction in changesToReport)
                {
                    reportAction.Invoke();
                }

                Console.WriteLine(new string('-', 40));
            }
        }
    }
}
