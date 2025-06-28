using System.Text.Json.Serialization;

namespace LolLcdApiClient.Models
{
    /// <summary>
    /// /liveclientdata/activeplayer のレスポンスをマッピングするクラス
    /// </summary>
    public class ActivePlayer
    {
        /// <summary>
        /// RiotID形式のサモナー名 (例: "ユーザー名#TAG")
        /// </summary>
        [JsonPropertyName("summonerName")]
        public string SummonerName { get; set; } = string.Empty;
    }
}
