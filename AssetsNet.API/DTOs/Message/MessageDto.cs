namespace AssetsNet.API.DTOs.Message;

public class MessageDto
{
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public string SenderId { get; set; }
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public string RecipientId { get; set; }
}