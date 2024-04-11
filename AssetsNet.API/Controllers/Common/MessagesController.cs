using System.Security.Claims;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers.Common;

[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IMessageRepository _messageRepository;

    public MessagesController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    [HttpGet("{recipientId}")]
    public async Task<ActionResult<List<Message>>> GetMessages(string recipientId)
    {
        string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        var messages = await _messageRepository.GetMessages(currentUserId, recipientId);

        return Ok(messages);
    }

    [HttpPost("send-message")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Message>> SendMessage(SendMessageDto sendMessageDto)
    {
        try
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var sentMessage = await _messageRepository.SendMessageAsync(currentUserId, sendMessageDto.RecipientId, sendMessageDto.Content);

            return Ok(sentMessage);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}