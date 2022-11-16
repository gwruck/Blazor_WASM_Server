using System.Net.Http.Headers;
using Blazor_WASM_Server.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Blazor_WASM_Server.Client
{
    public class HostHttpClient :IHostHttpClient
    {
        public HttpClient Client { get; }
        public IAccessTokenProvider _tokenProvider;
        public IConfiguration _config;

        public HostHttpClient(
            HttpClient client, 
            IHttpClientFactory clientFactory,
            NavigationManager navMan, 
            IAccessTokenProvider tokenProvider,
            IConfiguration config
            )
        {
            //Client = client.CreateClient();
            //Client = client;
            Client = clientFactory.CreateClient();
            Client.BaseAddress = new Uri(navMan.BaseUri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _tokenProvider = tokenProvider;
            _config = config;
        }

        public async Task<HttpClient> GetClientAsync()
        {
            var scopes = _config["AzureAd:WebApiScopes"];
            AccessTokenRequestOptions options = new();
            options.Scopes = new string[] { scopes };

            var accessTokenResult = await _tokenProvider.RequestAccessToken(options);

            string  accessToken = string.Empty;

            if (accessTokenResult.TryGetToken(out var token))
            {
                accessToken = token.Value;
            }

            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return Client;
        }
    }
}
