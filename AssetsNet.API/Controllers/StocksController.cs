using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.Stock;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class StocksController : BaseApiController
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("{stockName}")]
    public async Task<ActionResult> GetStockData([FromRoute] string stockName)
    {
        var stock = await _stockService.GetStockData(stockName);
        return Ok(stock);
    }
}