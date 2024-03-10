using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.ChatGpt;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class ChatGptController : BaseApiController
{
    private readonly IChatGptService _chatGptService;

    public ChatGptController(IChatGptService chatGptService)
    {
        _chatGptService = chatGptService;
    }

    [HttpGet("query")]
    public async Task<ActionResult> GetResponse([FromQuery] string query, [FromQuery] string? conversationId = null)
    {
        try
        {
            string response = await _chatGptService.QueryChatGpt(query, conversationId);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}