using AssetsNet.API.Models.Twitter;
using AssetsNet.API.Models.Twitter.TwitterUsersMedia;

namespace AssetsNet.API.Interfaces.Twitter;

public interface ITwitterService
{
    Task<IEnumerable<TwitterPost>> GetTwitterPosts(string query, int? searchType = null);
    Task<IEnumerable<TwitterUserMediaPost>> GetUserMedia(string screenName = "Stocktwits");
}