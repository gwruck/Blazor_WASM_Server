using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_WASM_Server.Client
{
    public class WebApiClient
    {
        public HttpClient Client { get; }

        public WebApiClient(HttpClient client)
        {
            Client = client;
        }
    
    }
}
