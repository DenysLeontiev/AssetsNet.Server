using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.Crypto;
using AssetsNet.API.Models.Crypto;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class CryptoController : BaseApiController
{
    private readonly ICryptoService _cryptoService;

    public CryptoController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpGet("{symbol}")]
    public async Task<ActionResult<CryptoCurrencyData>> GetBySymbol(string symbol)
    {
        var data = await _cryptoService.GetCryptoCurrencyData(symbol);

        return Ok(data);
    }
}