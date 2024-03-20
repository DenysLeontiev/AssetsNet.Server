using System.Text.Json;
using AssetsNet.API.Interfaces.Twitter;
using AssetsNet.API.Models.Twitter;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.Twitter;

/// <summary>
/// Api to fetch data from https://rapidapi.com/alexanderxbx/api/twitter-api45
/// </summary>
public class TwitterService : ITwitterService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public TwitterService(IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TwitterPost>> GetTwitterPosts(string query, int? searchType = null)
    {
        string url = searchType != null ? $"https://twitter-api45.p.rapidapi.com/search.php?query={query}&search_type={(TwitterSeacrhType)searchType}" :
            $"https://twitter-api45.p.rapidapi.com/search.php?query={query}";

        var apiKey = _configuration["TwitterRapidApi:X-RapidAPI-Key"]
            ?? throw new ArgumentNullException("TwitterRapidApi:X-RapidAPI-Key is not found in the configuration");

        var stockApiHost = _configuration["TwitterRapidApi:X-RapidAPI-Host"]
            ?? throw new ArgumentNullException("TwitterRapidApi:X-RapidAPI-Host is not found in the configuration");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
            {
                { "X-RapidAPI-Key", apiKey },
                { "X-RapidAPI-Host", stockApiHost },
            },
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        TwitterRootObject result = System.Text.Json.JsonSerializer.Deserialize<TwitterRootObject>(body, options)!;

        return result.Timeline;
    }
}