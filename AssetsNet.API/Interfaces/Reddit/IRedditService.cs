using AssetsNet.API.Helpers.Reddit;

namespace AssetsNet.API.Interfaces.Reddit;

public interface IRedditService
{
    Task<string> GetRedditPosts(string subreddit, int timePosted); // timePosted is enum RedditTimePosted.cs
}