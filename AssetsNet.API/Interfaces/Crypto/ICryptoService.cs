using AssetsNet.API.Models.Crypto;

namespace AssetsNet.API.Interfaces.Crypto;

public interface ICryptoService
{
    Task<CryptoCurrencyData> GetCryptoCurrencyData(string? symbol = null);
}