using AssetsNet.API.DTOs.ChatGpt;
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

    public async Task<ChatGptResponseDto> QueryChatGpt(string question, string userId, string? conversationId)
    {
        if (string.IsNullOrEmpty(question))
        {
            throw new ArgumentNullException(nameof(question));
        }

        var openAiKey = _configuration["ChatGpt:ApiKey"] 
            ?? throw new ArgumentNullException("ChatGpt:ApiKey");

        try
        {
            var openai = new ChatGpt(openAiKey);

            var response = await openai.Ask(question, conversationId);

            return new ChatGptResponseDto
            {
                Response = response,
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Error querying ChatGpt", ex);
        } 
    }
}