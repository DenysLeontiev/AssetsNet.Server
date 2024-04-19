using System.Security.Claims;
using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AssetsNet.API.Hubs;

public class MessageHub : Hub
{
    private readonly AssetsDbContext _context;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessageHub(AssetsDbContext context, IMessageRepository messageRepository, IMapper mapper)
    {
        _context = context;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        string currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var httpContext = Context.GetHttpContext(); // here we get HttpContext
        var otherUserId = httpContext.Request.Query["user"];
        var groupName = $"thread-{currentUserId}-{otherUserId}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // here we add new groupm for DMs(Direct Messages)

        var messages = await _messageRepository.GetMessages(currentUserId, otherUserId.ToString());
        var mappedMessages = _mapper.Map<List<MessageDto>>(messages);
        await Clients.Group(groupName).SendAsync("RecieveMessageThread", mappedMessages);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(SendMessageDto sendMessageDto) // sends message via hub
    {
        var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        if (currentUserId == sendMessageDto.RecipientId.ToLower())
        {
            throw new HubException("You cant send messages to yourself");
        }

        // var sender = await _userRepository.GetUserByUsernameAsync(username);
        // if (sender == null)
        // {
        //     throw new HubException("Sender is not found");
        // }

        // var recipient = await _userRepository.GetUserByUsernameAsync(sendMessageDto.RecipientUsername);
        // if (recipient == null)
        // {
        //     throw new HubException("Recipient is not found");
        // }

        var message = new Entities.Message
        {
            SenderId = currentUserId,
            RecipientId = sendMessageDto.RecipientId,
            Content = sendMessageDto.Content,
            DateSent = DateTime.UtcNow
        };

        await _context.Messages.AddAsync(message);

        await _context.SaveChangesAsync();

        var groupName = $"thread-{currentUserId}-{sendMessageDto.RecipientId}";

        // sends to message to thise who are listening to NewMessage
        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
    }
}