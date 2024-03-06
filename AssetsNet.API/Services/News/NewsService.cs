using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Models.News;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.News;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public NewsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IEnumerable<Models.News.News>> GetNewsAsync(string companyName, string region = "US")
    {
        var apiKey = _configuration.GetValue<string>("NewsRapindApi:X-RapidAPI-Key");
        var apiHost = _configuration.GetValue<string>("NewsRapindApi:X-RapidAPI-Host");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://apidojo-yahoo-finance-v1.p.rapidapi.com/auto-complete?q={companyName}&region={region}"),
            Headers =
            {
                { "X-RapidAPI-Key", apiKey },
                { "X-RapidAPI-Host", apiHost },
            },
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        RootObject result = JsonConvert.DeserializeObject<RootObject>(body)!;

        return result!.News;
    }
}