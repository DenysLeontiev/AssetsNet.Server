using AssetsNet.API.Interfaces.Stock;
using AssetsNet.API.Models.Stock;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.Stocks;

public class StockService : IStockService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public StockService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<StockData> GetStockData(string stockName)
    {
        var url = "https://stock-prices2.p.rapidapi.com/api/v1/resources/stock-prices/1d?ticker=" + stockName;
        var stockApiKey = _configuration["StocksRapidApi:X-RapidAPI-Key"]
            ?? throw new ArgumentNullException("StocksRapidApi:X-RapidAPI-Key is not found in the configuration");

        var stockApiHost = _configuration["StocksRapidApi:X-RapidAPI-Host"]
        ?? throw new ArgumentNullException("StocksRapidApi:X-RapidAPI-Host is not found in the configuration");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
                    {
                        { "X-RapidAPI-Key", stockApiKey },
                        { "X-RapidAPI-Host", stockApiHost },
                    },
        };
        using var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var dataToReturn = JsonConvert.DeserializeObject<Dictionary<string, Models.Stock.StockData>>(body);

        return dataToReturn!.First().Value;
    }

    public async Task<IEnumerable<HeaderStockData>> GetStockDataList(IEnumerable<string> stockNames)
    {
        var key = _configuration["StocksTwelveDataApi:StocksTwelveDataApiKey"];
        var baseUri = "https://api.twelvedata.com/quote";
        var stockDataList = new List<HeaderStockData>();

        foreach (var name in stockNames)
        {
            var queryString = $"symbol={Uri.EscapeDataString(name)}";
            var url = $"{baseUri}?{queryString}&apikey={key}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };

            try
            {
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<HeaderStockData>(body);

                if (data != null && data.Symbol != "")
                {
                    stockDataList.Add(data);
                }
                
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("Failed to get stock data from the API");
            }

        }

        return stockDataList;
    }
}