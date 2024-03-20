using AssetsNet.API.Models.Twitter;

namespace AssetsNet.API.Interfaces.Twitter;

public interface ITwitterService
{
    Task<string> GetTwitterPosts(string query, TwitterSeacrhType seacrhType);
}