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

            return response;
        }
        catch (Exception ex)
        {
            throw new Exception("Error querying ChatGpt", ex);
        } 
    }
}