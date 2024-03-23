using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.Stock;
using AssetsNet.API.Models.Stock;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class StocksController : BaseApiController
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpPost("stocks-list")]
    public async Task<ActionResult<List<HeaderStockData>>> GetStockData([FromBody] List<string> stockNames)
    {
        try
        {
            var stockData = await _stockService.GetStockDataList(stockNames);
            return Ok(stockData);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{stockName}")]
    public async Task<ActionResult> GetStockData([FromRoute] string stockName)
    {
        var stock = await _stockService.GetStockData(stockName);
        return Ok(stock);
    }
}