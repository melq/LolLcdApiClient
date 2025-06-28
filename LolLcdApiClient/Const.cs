namespace LolLcdApiClient
{
    /// <summary>
    /// 定数やエンドポイントを定義するクラス
    /// </summary>
    public class Const
    {
        /// <summary>
        /// League of Legends Live Client Data APIのエンドポイントを定義するクラス
        /// </summary>
        public class ApiUrls
        {
            public const string baseUrl = "https://127.0.0.1:2999/liveclientdata/";

            /// <summary>
            /// アクティブプレイヤーの情報を取得するAPIエンドポイント
            /// </summary>
            public const string activePlayerApiUrl = "activeplayer";

            /// <summary>
            /// プレイヤーリストを取得するAPIエンドポイント
            /// </summary>
            public const string playerListApiUrl = "playerlist";
        }
    }
}
