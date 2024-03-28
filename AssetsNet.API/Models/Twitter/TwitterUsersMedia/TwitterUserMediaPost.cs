using System.Text.Json.Serialization;

namespace AssetsNet.API.Models.Twitter.TwitterUsersMedia;

public class TwitterUserMediaPost
{
    [JsonPropertyName("tweet_id")]
    public string? TweetId { get; set; }
    public int? bookmarks { get; set; }
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    public int? Favorites { get; set; }
    public string? Text { get; set; }
    public string? Lang { get; set; }
    public string? Views { get; set; }
    public int? Quotes { get; set; }
    public int? Replies { get; set; }
    public int? Retweets { get; set; }
    [JsonPropertyName("conversation_id")]
    public string? ConversationId { get; set; }
    public TwitterUserMedia? Media { get; set; }
}
