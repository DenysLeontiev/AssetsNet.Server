using System.Security.Claims;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;
using AssetsNet.API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers.Common;

[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesController(IMessageRepository messageRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpGet("{recipientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(string recipientId)
    {
        string currentUserId = User.GetCurrentUserId();;

        var messages = await _messageRepository.GetMessages(currentUserId, recipientId);

        var mappedMessages = _mapper.Map<List<MessageDto>>(messages);

        return Ok(mappedMessages);
    }

    [HttpPost("send-message")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Message>> SendMessage(SendMessageDto sendMessageDto)
    {
        try
        {
            // string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            string currentUserId = User.GetCurrentUserId();
            var sentMessage = await _messageRepository.SendMessageAsync(currentUserId, sendMessageDto.RecipientId, sendMessageDto.Content);

            return Ok(sentMessage);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}