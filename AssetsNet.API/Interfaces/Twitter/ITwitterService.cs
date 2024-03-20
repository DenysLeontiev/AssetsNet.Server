using AssetsNet.API.Models.Twitter;

namespace AssetsNet.API.Interfaces.Twitter;

public interface ITwitterService
{
    Task<IEnumerable<TwitterPost>> GetTwitterPosts(string query, int? searchType = null);
}