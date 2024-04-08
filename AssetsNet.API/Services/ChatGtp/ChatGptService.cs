using AssetsNet.API.Data;
using AssetsNet.API.DTOs.ChatGpt;
using AssetsNet.API.Interfaces.ChatGpt;
using ChatGPT.Net;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Services.ChatGtp;

public class ChatGptService : IChatGptService
{
    private readonly IConfiguration _configuration;
    private readonly AssetsDbContext _dbContext;

    public ChatGptService(IConfiguration configuration, AssetsDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<ChatGptResponseDto> QueryChatGpt(string question, string userId, string? conversationId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        if (user.GptRequestsLeft <= 0)
        {
            throw new GptRequestsLimitExceededException("Gpt requests limit exceeded");
        }

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

            user.GptRequestsLeft--;

            await _dbContext.SaveChangesAsync();

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