using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Message;
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


    public async Task<MessageDto> SendMessageAsync(string currentUserId, string recipientId, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentNullException("You cannot send empty messages");
        }

        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(recipientId))
        {
            throw new ArgumentNullException("Sender or Recipient is null");
        }

        if (currentUserId.Equals(recipientId))
        {
            throw new Exception("You cannot send message to yourself");
        }

        var currentUser = await _context.Users.FindAsync(currentUserId)
            ?? throw new Exception("User is not found");

        var recipientUser = await _context.Users.FindAsync(recipientId)
            ?? throw new Exception("User is not found");

        var message = new Entities.Message
        {
            SenderId = currentUserId,
            RecipientId = recipientId,
            Content = content,
            DateSent = DateTime.UtcNow
        };

        await _context.Messages.AddAsync(message);

        await _context.SaveChangesAsync();

        var messageToReturn = new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            DateSent = DateTime.UtcNow,
        };

        return messageToReturn;
    }

    public async Task<List<Entities.Message>> GetMessages(string senderId, string recipientId)
    {
        // var messages = await _context.Messages.Include(x => x.Sender).Include(x => x.Recipient)
        // .Where(x => x.SenderId.Equals(senderId) && x.RecipientId.Equals(recipientId)
        //     || (x.SenderId.Equals(recipientId) && x.RecipientId.Equals(senderId)))
        //                                       .OrderBy(x => x.DateSent)
        //                                       .ToListAsync();

        var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.ProfilePhoto)
                .Include(u => u.Recipient).ThenInclude(p => p.ProfilePhoto)
                .Where(m => m.RecipientId == recipientId && m.SenderId == senderId
                    || m.RecipientId == senderId && m.SenderId == recipientId
                )
                .OrderByDescending(m => m.DateSent)
                .ToListAsync();

        return messages;
    }
}