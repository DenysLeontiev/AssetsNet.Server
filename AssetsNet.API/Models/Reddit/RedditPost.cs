namespace AssetsNet.API.Models.Reddit;

public class RedditPost
{
    public string Id { get; set; } 
    public string Title { get; set; }
    public int NumComments { get; set; }
    public int Score { get; set; }
    public float UpvoteRatio { get; set; }
    public long Created { get; set; }
    public string Author { get; set; }
    public string AuthorId { get; set; }
    public string PermaLink { get; set; }
    public RedditThumbnail Thumbnail { get; set; }
}
