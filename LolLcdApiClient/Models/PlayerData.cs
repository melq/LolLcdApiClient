using LolLcdApiClient.Converters;
using System.Text.Json.Serialization;

namespace LolLcdApiClient.Models
{
    /// <summary>
    /// /liveclientdata/playerlist の各プレイヤー要素をマッピングするクラス
    /// </summary>
    public class PlayerData
    {
        /// <summary>
        /// チャンピオン名
        /// </summary>
        [JsonPropertyName("championName")]
        public string ChampionName { get; set; } = string.Empty;

        /// <summary>
        /// サモナー名
        /// </summary>
        [JsonPropertyName("summonerName")]
        public string SummonerName { get; set; } = string.Empty;

        /// <summary>
        /// チーム名 ("ORDER" または "CHAOS"
        /// </summary>
        [JsonPropertyName("team")]
        public string Team { get; set; } = string.Empty;

        /// <summary>
        /// ポジション名 ("JUNGLE", "TOP" など
        /// </summary>
        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// レベル
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>
        /// アイテムのリスト
        /// </summary>
        [JsonPropertyName("items")]
        [JsonConverter(typeof(CustomListConverter<PlayerItem>))]
        public List<PlayerItem> Items { get; set; } = [];

        /// <summary>
        /// スコア情報
        /// </summary>
        [JsonPropertyName("scores")]
        [JsonConverter(typeof(CustomObjectConverter<PlayerScores>))]
        public PlayerScores Scores { get; set; } = new PlayerScores();
    }
}
