using LolLcdApiClient.Models;
using LolLcdApiClient.Util.Interfaces;
using System.Text.Json;

namespace LolLcdApiClient.Util
{
    /// <summary>
    /// <inheritdoc cref="ILiveClientDataApiClient"/>"/>
    /// </summary>
    public class LiveClientDataApiClient : ILiveClientDataApiClient
    {
        private readonly string getPlayerListEndpoint = Const.ApiUrls.playerListApiUrl;
        private readonly string getActivePlayerEndpoint = Const.ApiUrls.activePlayerApiUrl;
        private readonly HttpClient _client;

        public LiveClientDataApiClient()
        {
            // 自己署名証明書の検証を無効にするためのハンドラ
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(Const.ApiUrls.baseUrl)
            };
        }

        /// <inheritdoc/>
        public async Task<string> GetStringAsync(string endpoint)
        {
            return await _client.GetStringAsync(endpoint);
        }

        /// <inheritdoc/>
        public async Task<List<PlayerData>> GetPlayerListAsync()
        {
            var json = await GetStringAsync(getPlayerListEndpoint);
            return JsonSerializer.Deserialize<List<PlayerData>>(json) ?? [];
        }

        /// <inheritdoc/>
        public async Task<ActivePlayer> GetActivePlayerAsync()
        {
            var json = await GetStringAsync(getActivePlayerEndpoint);
            return JsonSerializer.Deserialize<ActivePlayer>(json) ?? new ActivePlayer();
        }
    }
}
