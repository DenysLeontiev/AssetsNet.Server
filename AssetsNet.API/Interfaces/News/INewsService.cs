using AssetsNet.API.Models.News;

namespace AssetsNet.API.Interfaces.News;

public interface INewsService
{
    Task<Models.News.News> GetNews(string companyName, string region = "US");    
}