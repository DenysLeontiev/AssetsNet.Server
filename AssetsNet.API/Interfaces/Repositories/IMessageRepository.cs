using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<Message> SendMessageAsync(string currentUserId, string recipientId, string content);
    Task<List<Entities.Message>> GetMessages(string senderId, string recipientId);
    Task<List<Conversation>> GetConversations(); 
}

public class Conversation 
{
    public string SenderName { get; set; }
    public string SenderId { get; set; }
    public string RecipientName { get; set; }
    public string RecipientId { get; set; }
}