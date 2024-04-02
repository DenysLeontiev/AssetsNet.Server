using AssetsNet.API.Models.News;
using NewsAPI.Models;

namespace AssetsNet.API.Interfaces.News;

public interface INewsService
{
    Task<IEnumerable<Models.News.News>> GetNewsAsync(string companyName, string region = "US");
    Task<IEnumerable<Article>> GetNewsApiArticles(string query);
}