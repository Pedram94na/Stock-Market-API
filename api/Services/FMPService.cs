using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public FMPService(HttpClient httpCLient, IConfiguration config)
        {
            this.httpClient = httpCLient;
            this.config = config;    
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={config["FMPKey"]}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];

                    return stock is null ? null : stock.ToStockFromFMP();
                }
            }

            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }

            return null;
        }
    }
}