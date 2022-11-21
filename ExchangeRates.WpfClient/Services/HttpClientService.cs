using System;
using System.Net.Http;

namespace ExchangeRates.WpfClient.Services
{
    public static class HttpClientService
    {
        public static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7195/api/");
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }
    }
}
