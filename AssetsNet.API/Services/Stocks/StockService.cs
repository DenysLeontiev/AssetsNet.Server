using AssetsNet.API.Interfaces.Stock;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.Stocks;

public class StockService : IStockService
{
    private readonly IConfiguration _configuration;

    public StockService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Models.Stock.StockData> GetStockData(string stockName)
    {

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://stock-prices2.p.rapidapi.com/api/v1/resources/stock-prices/1d?ticker=" + stockName),
            Headers =
        {
            { "X-RapidAPI-Key", "e93439e715msh56fb70def110954p1f8153jsn4cdb8cbdbce2" },
            { "X-RapidAPI-Host", "stock-prices2.p.rapidapi.com" },
        },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            var data = JsonConvert.DeserializeObject<Dictionary<string, Models.Stock.StockData>>(body);
        }

        return null;
    }
}