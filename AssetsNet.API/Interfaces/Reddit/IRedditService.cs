using AssetsNet.API.Models.Reddit;

namespace AssetsNet.API.Interfaces.Reddit;

public interface IRedditService
{
    Task<IEnumerable<RedditPost>> GetRedditPosts(string subreddit, int timePosted, string userId); // timePosted is enum RedditTimePosted.cs
}