using LolLcdApiClient.Models;

namespace LolLcdApiClient.Util.Interfaces
{
    /// <summary>
    /// Live Client Data APIを操作するクライアントクラス
    /// </summary>
    public interface ILiveClientDataApiClient
    {
        /// <summary>
        /// 指定されたエンドポイントから文字列を非同期に取得します。
        /// </summary>
        /// <param name="endpoint">エンドポイント</param>
        /// <returns>取得した文字列</returns>
        Task<string> GetStringAsync(string endpoint);

        /// <summary>
        /// <see cref="PlayerData"/>のリストを非同期に取得します。"/>
        /// </summary>
        /// <returns>取得した<see cref="PlayerData"/>リスト</returns>
        Task<List<PlayerData>> GetPlayerListAsync();

        /// <summary>
        /// <see cref="ActivePlayer"/>を非同期に取得します。"/>
        /// </summary>
        /// <returns>取得した<see cref="ActivePlayer"/></returns>
        Task<ActivePlayer> GetActivePlayerAsync();
    }
}