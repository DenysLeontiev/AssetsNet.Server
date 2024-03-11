using AssetsNet.API.Interfaces.Crypto;
using AssetsNet.API.Models.Crypto;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.Crypto;

public class CryptoService : ICryptoService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public CryptoService(IConfiguration configuration,
                         HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<CryptoCurrencyData> GetCryptoCurrencyData(string? symbol = null)
    {
        var apiKey = _configuration["Aletheia:ApiKey"]
            ?? throw new ArgumentNullException("Aletheia:ApiKey is null");

        var apiUrl = "https://api.aletheiaapi.com/Crypto?symbol=" + symbol;

        if (string.IsNullOrEmpty(symbol))
        {
            throw new ArgumentNullException("Crypto symbol is required");
        }


        // send apiKey key in header with each request
        _httpClient.DefaultRequestHeaders.Add("key", apiKey);

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CryptoCurrencyData>(responseData);

            return data!;
        }
        else
        {
            throw new Exception($"An error occured while retrieving crypto({symbol})");
        }
    }
}