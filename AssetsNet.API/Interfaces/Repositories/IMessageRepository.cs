using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages(string senderId, string recipientId); 
}