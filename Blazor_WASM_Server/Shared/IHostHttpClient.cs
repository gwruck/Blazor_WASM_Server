using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_WASM_Server.Shared
{
    public interface IHostHttpClient
    {
        public Task<HttpClient> GetClientAsync();

    }
}
