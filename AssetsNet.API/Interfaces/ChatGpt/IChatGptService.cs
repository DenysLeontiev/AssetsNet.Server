namespace AssetsNet.API.Interfaces.ChatGpt;

public interface IChatGptService
{
    Task<string> QueryChatGpt(string question, string? conversationId);
}