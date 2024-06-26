using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Models.News;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
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

    public async Task<IEnumerable<Models.News.News>> GetNewsAsync(string companyName, string region)
    {
        var apiKey = _configuration["NewsRapidApi:X-RapidAPI-Key"]
            ?? throw new ArgumentNullException("NewsRapidApi:X-RapidAPI-Key is not found in the configuration");
        var apiHost = _configuration["NewsRapidApi:X-RapidAPI-Host"]
            ?? throw new ArgumentNullException("NewsRapidApi:X-RapidAPI-Host is not found in the configuration");

        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentNullException(nameof(companyName));
        }

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

        try
        {
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            RootObject result = JsonConvert.DeserializeObject<RootObject>(body)!;

            return result!.News;
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException($"Error while getting news for {companyName} from Yahoo Finance API", e);
        }
    }

    // API Docs https://newsapi.org/docs/client-libraries/csharp
    public async Task<IEnumerable<Article>> GetNewsApiArticles(string query) 
    {
        var newsApiClient = new NewsApiClient(_configuration["NewsApiKey"]);
        List<Article> articles = new();

        try
        {
            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = query,
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                PageSize = 9,
                From = DateTime.Now.AddDays(-5) // get previous month
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                Console.WriteLine(articlesResponse.TotalResults);
                foreach (var article in articlesResponse.Articles)
                {
                    if (article is not null)
                    {
                        articles.Add(article);
                    }
                }
            }
            return articles;
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException($"Error while getting news for {query} from Yahoo NewsAPI", e);
        }
    }
}