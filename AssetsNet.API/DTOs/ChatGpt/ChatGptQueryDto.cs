namespace AssetsNet.API.DTOs.ChatGpt;
public class ChatGptQueryDto
{
    public string Query { get; set; } = string.Empty;
    public string? ConversationId { get; set; }
}