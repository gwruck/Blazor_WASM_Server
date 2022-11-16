using System.Net.Http.Headers;
using Blazor_WASM_Server.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;

namespace Blazor_WASM_Server.Server
{
    public class HostHttpClient :IHostHttpClient
    {
        private HttpClient _client { get; }
        private ITokenAcquisition _tokenAcquisition;
        private MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;
        private string? token;
        public IConfiguration _config;
        public HostHttpClient(
            HttpClient client, 
            //IHttpClientFactory clientFactory,
            NavigationManager navMan, 
            ITokenAcquisition tokenAcquisition,
            MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler,
            IConfiguration config
            )
        {
            //Client = client.CreateClient();
            //var client1 = clientFactory.CreateClient();
            _client = client;
            _client.BaseAddress = new Uri(navMan.BaseUri);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _tokenAcquisition = tokenAcquisition;
            _config = config;
            _consentHandler = consentHandler;

        }


        public async Task<HttpClient> GetClientAsync()
        {
            if (token == null)
            {
                try
                {
                    var scopes = _config["AzureAd:Scopes"];
                    token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { scopes });

                }
                catch (Exception e)
                {
                    _consentHandler.HandleException(e);
                    //Console.WriteLine(e);
                    //throw;
                }
            }

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return _client;
        }

    }
}
