using System.Text.Json.Serialization;

namespace AssetsNet.API.Models.Twitter.TwitterUsersMedia;

public class TwitterUserMediaPhoto
{
    [JsonPropertyName("media_url_https")]
    public string? MediaUrl { get; set; }
    public string? Id { get; set; }
}