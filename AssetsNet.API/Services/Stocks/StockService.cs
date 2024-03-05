using Aletheia.Service;
using Aletheia.Service.StockData;
using AssetsNet.API.Interfaces.Stock;
using AssetsNet.API.Models.Stock;

namespace AssetsNet.API.Services.Stocks;

public class StockService : IStockService
{
    private readonly IConfiguration _configuration;

    public StockService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Stock> GetStockData(string stockName)
    {
        string aletheiaApiKey = _configuration.GetValue<string>("AletheiaApiKey");
        AletheiaService service = new AletheiaService(aletheiaApiKey);
        StockData quote = await service.GetStockDataAsync(stockName, true, true); // AAPL

        var stock = new Stock(
            quote.SummaryData.Name,
            quote.SummaryData.Price,
            quote.SummaryData.MarketCap,
            quote.SummaryData.DollarChange,
            quote.SummaryData.PercentChange);

        return stock;
    }
}