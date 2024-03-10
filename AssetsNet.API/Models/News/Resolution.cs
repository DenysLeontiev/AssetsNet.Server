namespace AssetsNet.API.Models.News;

public class Resolution
{
    public string Url { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public string Tag { get; set; } = string.Empty;
}