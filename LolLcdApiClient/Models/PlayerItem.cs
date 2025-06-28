using System.Text.Json.Serialization;

namespace LolLcdApiClient.Models
{
    /// <summary>
    /// プレイヤーのアイテム情報を表すクラス
    /// </summary>
    public class PlayerItem
    {
        /// <summary>
        /// アイテムID
        /// </summary>
        [JsonPropertyName("itemID")]
        public int ItemID { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        [JsonPropertyName("displayName")]
        public string ItemName { get; set; } = string.Empty;
    }
}