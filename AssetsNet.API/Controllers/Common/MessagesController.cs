using System.Security.Claims;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers.Common;

public class MessagesController : BaseApiController
{
    private readonly IMessageRepository _messageRepository;

    public MessagesController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    [Authorize]
    [HttpPost("send-message")]
    public async Task<Message> SendMessage(SendMessageDto sendMessageDto)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var sentMessage = await _messageRepository.SendMessageAsync(userId, sendMessageDto.RecipientId, sendMessageDto.Content);

        return sentMessage;
    }
}