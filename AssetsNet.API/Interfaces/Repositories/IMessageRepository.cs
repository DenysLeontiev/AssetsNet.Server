using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<MessageDto> SendMessageAsync(string currentUserId, string recipientId, string content);
    Task<List<Message>> GetMessages(string senderId, string recipientId); 
}