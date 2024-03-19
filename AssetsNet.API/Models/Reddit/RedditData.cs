namespace AssetsNet.API.Models.Reddit;

public class RedditData
{
    public string Cursor { get; set; }
    public List<RedditPost> Posts { get; set; }
}
