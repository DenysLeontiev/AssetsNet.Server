using System.Text.Json.Serialization;

namespace AssetsNet.API.Models.Twitter;

public class TwitterPost
{
    public string? Type { get; set; }
    [JsonPropertyName("tweet_id")]
    public string? TweetId { get; set; }
    public string? Text { get; set; }
    [JsonPropertyName("created_at")]
    public string? Created { get; set; }
    public string? Views { get; set; }
}