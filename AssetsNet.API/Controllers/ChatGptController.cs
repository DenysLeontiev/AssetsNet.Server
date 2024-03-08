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
    public async Task<ActionResult> GetResponse([FromRoute] string query, [FromRoute] string? conversationId)
    {
        string response = await _chatGptService.QueryChatGpt(query, conversationId);

        return Ok(response);
    }
}