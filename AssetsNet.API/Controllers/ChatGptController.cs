using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs.ChatGpt;
using AssetsNet.API.Interfaces.ChatGpt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class ChatGptController : BaseApiController
{
    private readonly IChatGptService _chatGptService;

    public ChatGptController(IChatGptService chatGptService)
    {
        _chatGptService = chatGptService;
    }

    [Authorize]
    [HttpPost("query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatGptResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChatGptResponseDto>> GetResponse([FromBody] ChatGptQueryDto queryDto)
    {
        try
        {
            var response = await _chatGptService.QueryChatGpt(queryDto.Query, queryDto.ConversationId);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}