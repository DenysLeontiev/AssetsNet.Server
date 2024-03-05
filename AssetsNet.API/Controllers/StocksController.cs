using Aletheia.Service;
using Aletheia.Service.StockData;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.Stock;
using AssetsNet.API.Models.Stock;
using AssetsNet.API.Services.Stocks;
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
    public async Task<ActionResult<Stock>> GetStockData([FromRoute] string stockName)
    {
        var stock = await _stockService.GetStockData(stockName);
        return Ok(stock);
    }
}