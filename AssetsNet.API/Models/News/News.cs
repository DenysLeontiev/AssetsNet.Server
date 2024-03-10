namespace AssetsNet.API.Models.News;

public class News
{
    public string Uuid { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public long ProviderPublishTime { get; set; }
    public string Type { get; set; } = string.Empty;
    public Thumbnail Thumbnail { get; set; } = new();
    public List<string> RelatedTickers { get; set; } = new();
}