using System.Text.Json;
using AssetsNet.API.Data;
using AssetsNet.API.Interfaces.Twitter;
using AssetsNet.API.Models.Twitter;
using AssetsNet.API.Models.Twitter.TwitterUsersMedia;

namespace AssetsNet.API.Services.Twitter;

/// <summary>
/// Api to fetch data from https://rapidapi.com/alexanderxbx/api/twitter-api45
/// </summary>
public class TwitterService : ITwitterService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly AssetsDbContext _dbContext;

    public TwitterService(IConfiguration configuration,
        HttpClient httpClient,
        AssetsDbContext dbContext)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TwitterPost>> GetTwitterPosts(string query, string userId, int? searchType = null)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user!.GptRequestsLeft <= 0)
        {
            throw new GptRequestsLimitExceededException("Gpt requests limit exceeded");
        }

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

        try
        {

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            TwitterRootObject result = JsonSerializer.Deserialize<TwitterRootObject>(body, options)!;

            return result.Timeline;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Error while getting tweets for '{query}' from Twitter", ex);
        }
    }

    public async Task<IEnumerable<TwitterUserMediaPost>> GetUserMedia(string screenName = "Stocktwits")
    {
        var url = "https://twitter-api45.p.rapidapi.com/usermedia.php?screenname=" + screenName;

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

        try
        {
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            TwitterUsersMediaRootObject result = JsonSerializer.Deserialize<TwitterUsersMediaRootObject>(body, options)!;

            return result.Timeline;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Error while getting tweets from '{screenName}' from Twitter", ex);
        }
    }
}