using System.Security.Claims;
using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;
using AssetsNet.API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AssetsNet.API.Hubs;

public class MessageHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessageHub(IMessageRepository messageRepository, 
                      IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        string currentUserId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var httpContext = Context.GetHttpContext(); // here we get HttpContext
        var recipientUserId = httpContext!.Request.Query["user"];
        var groupName = GetGroupName(currentUserId, recipientUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // here we add new groupm for DMs(Direct Messages)

        var messages = await _messageRepository.GetMessages(currentUserId, recipientUserId.ToString());
        var mappedMessages = _mapper.Map<List<MessageDto>>(messages);
        await Clients.Group(groupName).SendAsync("RecieveMessageThread", mappedMessages);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(SendMessageDto sendMessageDto) // sends message via hub
    {
        var currentUserId = Context.User!.GetCurrentUserId();

        var message = await _messageRepository.SendMessageAsync(currentUserId, sendMessageDto.RecipientId, sendMessageDto.Content);

        var groupName = GetGroupName(currentUserId, sendMessageDto.RecipientId);

        // sends to message to thise who are listening to NewMessage
        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
    }

  private string GetGroupName(string senderId, string recipientId)
  {
      var sortedIds = new List<string> { senderId, recipientId }.OrderBy(id => id);
    
      return $"user-messages-{string.Join("-", sortedIds)}";
  }
}