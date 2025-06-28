using System.Text.Json.Serialization;

namespace LolLcdApiClient.Models
{

    /// <summary>
    /// プレイヤーのスコア情報を表すクラス
    /// </summary>
    public class PlayerScores
    {
        /// <summary>
        /// キル数
        /// </summary>
        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        /// <summary>
        /// デス数
        /// </summary>
        [JsonPropertyName("deaths")]
        public int Deaths { get; set; }

        /// <summary>
        /// アシスト数
        /// </summary>
        [JsonPropertyName("assists")]
        public int Assists { get; set; }

        /// <summary>
        /// CS数
        /// </summary>
        [JsonPropertyName("creepScore")]
        public int CreepScore { get; set; }
    }
}