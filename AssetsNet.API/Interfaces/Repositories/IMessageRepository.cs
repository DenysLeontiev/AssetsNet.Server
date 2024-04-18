using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<Message> SendMessageAsync(string currentUserId, string recipientId, string content);
    Task<List<Message>> GetMessages(string senderId, string recipientId); 
}