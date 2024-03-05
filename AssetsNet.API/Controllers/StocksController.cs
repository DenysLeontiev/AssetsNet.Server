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
            quote.SummaryData.Price,
            quote.SummaryData.MarketCap,
            quote.SummaryData.DollarChange,
            quote.SummaryData.PercentChange);

        return Ok(stock);
    }
}