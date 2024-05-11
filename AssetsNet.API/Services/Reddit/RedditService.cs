using AssetsNet.API.Data;
using AssetsNet.API.Helpers.Reddit;
using AssetsNet.API.Interfaces.Reddit;
using AssetsNet.API.Models.Reddit;
using Newtonsoft.Json;

namespace AssetsNet.API.Services.Reddit;

public class RedditService : IRedditService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly AssetsDbContext _dbContext;

    public RedditService(IConfiguration configuration, HttpClient httpClient,
        AssetsDbContext dbContext)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<RedditPost>> GetRedditPosts(string subreddit, int timePosted, string userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user!.GptRequestsLeft <= 0)
        {
            throw new GptRequestsLimitExceededException("Gpt requests limit exceeded");
        }

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
        RedditRootObject result = JsonConvert.DeserializeObject<RedditRootObject>(body)!;

        return result!.Data!.Posts!;
    }
}