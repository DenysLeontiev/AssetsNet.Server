namespace AssetsNet.API.Models.News;

public class RootObject
{
    public List<object> Explains { get; set; }
    public int Count { get; set; }
    public List<Quote> Quotes { get; set; }
    public List<News> News { get; set; }
    // Add other properties as needed
}