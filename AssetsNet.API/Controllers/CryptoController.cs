using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.Crypto;
using AssetsNet.API.Models.Crypto;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class CryptoController : BaseApiController
{
    private readonly ICryptoService _cryptoService;
    private readonly ILogger<CryptoController> _logger;

    public CryptoController(ICryptoService cryptoService,
    ILogger<CryptoController> logger)
    {
        _cryptoService = cryptoService;
        _logger = logger;
    }

    [HttpGet("{symbol}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CryptoCurrencyData>> GetBySymbol(string symbol)
    {
        try
        {
            var data = await _cryptoService.GetCryptoCurrencyData(symbol);

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, $"An error occured while getting crypto({symbol})");
            return BadRequest("An error occured while getting crypto");
            throw;
        }
    }
}