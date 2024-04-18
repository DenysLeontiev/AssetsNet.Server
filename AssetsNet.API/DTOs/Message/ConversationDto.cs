namespace AssetsNet.API.DTOs.Message;

public class ConversationDto
{
    public string InterlocutorId { get; set; }
    // Optionally include user information
    public string InterlocutorName { get; set; }
    public List<MessageDto> Messages { get; set; }
}