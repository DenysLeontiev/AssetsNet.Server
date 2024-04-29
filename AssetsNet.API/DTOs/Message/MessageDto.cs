namespace AssetsNet.API.DTOs.Message;

public class MessageDto
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DateSent { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
    public string RecipientPhotoUrl { get; set; } = string.Empty;
    public string SenderPhotoUrl { get; set; } = string.Empty;
}