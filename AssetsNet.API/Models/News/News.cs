namespace AssetsNet.API.Models.News;

public class News
{
    public string Uuid { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }
    public string Link { get; set; }
    public long ProviderPublishTime { get; set; }
    public string Type { get; set; }
    public Thumbnail Thumbnail { get; set; }
    public List<string> RelatedTickers { get; set; }
}