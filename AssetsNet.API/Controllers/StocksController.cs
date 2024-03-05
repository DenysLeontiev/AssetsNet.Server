using Aletheia.Service;
using Aletheia.Service.StockData;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Models.Stock;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class StocksController : BaseApiController
{
    private readonly IConfiguration _configuration;

    public StocksController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("{stockName}")]
    public async Task<ActionResult<Stock>> GetStockData([FromRoute] string stockName)
    {
        string aletheiaApiKey = _configuration.GetValue<string>("AletheiaApiKey");
        AletheiaService service = new AletheiaService(aletheiaApiKey);
        StockData quote = await service.GetStockDataAsync(stockName, true, true); // AAPL

        var stock = new Stock(
            quote.SummaryData.Name,
            quote.SummaryData.Price.ToString("#,##0.00"),
            quote.SummaryData.MarketCap.ToString("#,##0"),
            quote.SummaryData.DollarChange.ToString("#,##0.00"),
            quote.SummaryData.PercentChange.ToString("#0.0%"));

        Console.WriteLine("Name: " + quote.SummaryData.Name);
        Console.WriteLine("Price: " + quote.SummaryData.Price.ToString("#,##0.00"));
        Console.WriteLine("MarketCap: " + quote.SummaryData.MarketCap.ToString("#,##0"));
        Console.WriteLine("Change $: " + quote.SummaryData.DollarChange.ToString("#,##0.00"));
        Console.WriteLine("Change %: " + quote.SummaryData.PercentChange.ToString("#0.0%"));

        return Ok(stock);
    }
}