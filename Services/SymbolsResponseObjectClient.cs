using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Case.Model;

namespace Case.Services
{
    public class SymbolsResponseObjectClient
    {
        private readonly HttpClient _httpClient;
        public SymbolsResponseObjectClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SymbolsResponseObject> GetCurrentResponse()
        {
            return await _httpClient.GetFromJsonAsync<SymbolsResponseObject>("https://api.twelvedata.com/time_series?symbol=AAPL,MSFT,EUR/USD,SBUX,NKE&interval=1min&apikey=demo&source=docs");
        }

    }
}