namespace LolLcdApiClient.Services.Interfaces
{
    /// <summary>
    /// 相手ジャングラーの情報を監視するサービス
    /// </summary>
    public interface IJungleTrackerService
    {
        /// <summary>
        /// 監視を開始します。
        /// </summary>
        /// <returns></returns>
        Task StartTrackingAsync();
    }
}