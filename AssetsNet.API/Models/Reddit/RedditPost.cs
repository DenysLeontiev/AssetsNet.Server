namespace AssetsNet.API.Models.Reddit;

public class RedditPost
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int NumComments { get; set; }
    public int Score { get; set; }
    public float UpvoteRatio { get; set; }
    public long Created { get; set; }
    public string Author { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string PermaLink { get; set; } = string.Empty;
    public RedditThumbnail? Thumbnail { get; set; }
}
