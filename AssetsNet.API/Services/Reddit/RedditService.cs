using AssetsNet.API.Helpers.Reddit;
using AssetsNet.API.Interfaces.Reddit;

namespace AssetsNet.API.Services.Reddit;

public class RedditService : IRedditService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public RedditService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> GetRedditPosts(string subreddit, int timePosted)
    {
        var stockApiKey = _configuration["RedditRapidApi:X-RapidAPI-Key"]
            ?? throw new ArgumentNullException("RedditRapidApi:X-RapidAPI-Key is not found in the configuration");

        var stockApiHost = _configuration["RedditRapidApi:X-RapidAPI-Host"]
        ?? throw new ArgumentNullException("RedditRapidApi:X-RapidAPI-Host is not found in the configuration");

        RedditTimePosted subredditTimePosted = (RedditTimePosted)timePosted;
        var url = $"https://reddit34.p.rapidapi.com/getTopPostsBySubreddit?subreddit={subreddit}&time={subredditTimePosted}";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
            {
                { "X-RapidAPI-Key", stockApiKey },
                { "X-RapidAPI-Host", stockApiHost },
            },
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        return body;
    }
}