namespace AssetsNet.API.Helpers;

public class Conversation
{
    public string SenderName { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string SenderPhotoUrl { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
    public string RecipientPhotoUrl { get; set; } = string.Empty;
}