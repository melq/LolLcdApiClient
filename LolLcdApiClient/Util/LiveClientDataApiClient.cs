using LolLcdApiClient.Util.Interfaces;

namespace LolLcdApiClient.Util
{
    /// <summary>
    /// <inheritdoc cref="ILiveClientDataApiClient"/>"/>
    /// </summary>
    public class LiveClientDataApiClient : ILiveClientDataApiClient
    {
        private readonly HttpClient _client;

        public LiveClientDataApiClient()
        {
            // 自己署名証明書の検証を無効にするためのハンドラ
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler);

            _client.BaseAddress = new Uri(Const.ApiUrls.baseUrl);
        }

        /// <inheritdoc/>
        public async Task<string> GetStringAsync(string endpoint)
        {
            return await _client.GetStringAsync(endpoint);
        }
    }
}
