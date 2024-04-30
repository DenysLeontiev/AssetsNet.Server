using AssetsNet.API.DTOs.ChatGpt;

namespace AssetsNet.API.Interfaces.ChatGpt;

public interface IChatGptService
{
    Task<ChatGptResponseDto> QueryChatGpt(string question, string userId, string? conversationId);
}