using AssetsNet.API.Interfaces.ChatGpt;
using ChatGPT.Net;

namespace AssetsNet.API.Services.ChatGtp;

public class ChatGptService : IChatGptService
{
    private readonly IConfiguration _configuration;

    public ChatGptService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> QueryChatGpt(string question, string? conversationId)
    {
        var openAiKey = _configuration["ChatCgpt:ApiKey"];

        var openai = new ChatGpt(openAiKey);

        var response = await openai.Ask(question, conversationId);

        return response;
    }
}