using AssetsNet.API.Data;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Repositories.Message;

public class MessageRepository : IMessageRepository
{
    private readonly AssetsDbContext _context;

    public MessageRepository(AssetsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Entities.Message>> GetMessages(string senderId, string recipientId)
    {
        var messages = await _context.Messages.Include(x => x.Sender).Include(x => x.Recipient).Where(x => x.SenderId.Equals(senderId) && x.RecipientId.Equals(recipientId) 
            || (x.SenderId.Equals(recipientId) && x.RecipientId.Equals(senderId)))
                                              .OrderByDescending(x => x.DateSent)
                                              .ToListAsync();

        return messages;
    }
}