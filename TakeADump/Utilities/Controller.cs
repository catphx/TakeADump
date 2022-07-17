using System.Net.Http.Headers;
using TakeADump.Enums;

namespace TakeADump.Utilities
{
    internal class Controller : IController
    {
        private string ApiKey { get; set; }
        private string ApiUrl { get; set; }
        private AuthType AuthType { get; set; }

        public Controller(string ApiKey, string ApiUrl, AuthType AuthType)
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentException($"'{nameof(ApiKey)}' cannot be null or empty.", nameof(ApiKey));
            }
            if (string.IsNullOrEmpty(ApiUrl))
            {
                throw new ArgumentException($"'{nameof(ApiUrl)}' cannot be null or empty.", nameof(ApiUrl));
            }
            this.ApiKey = ApiKey;
            this.ApiUrl = ApiUrl;
            this.AuthType = AuthType;
        }

        public async Task<byte[]> SendAsync(HttpMethod httpMethod)
        {
            var httpClient = new HttpClient(new HttpClientHandler()
            {
                UseCookies = false,
                AllowAutoRedirect = true
            });

            switch (AuthType)
            {
                case AuthType.APIKEY:
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", ApiKey);
                    break;
                case AuthType.USERPASS:
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {ApiKey.ToBase64()}");
                    break;
                default:
                    break;
            }

            using var sendRequest = new HttpRequestMessage(httpMethod, new Uri(ApiUrl));

            var response = await httpClient.SendAsync(sendRequest);

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
