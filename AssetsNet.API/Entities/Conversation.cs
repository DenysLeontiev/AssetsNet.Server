namespace AssetsNet.API.Entities;

public class Conversation
{
    public string Id { get; set; }
    public string FirstConversationalistId { get; set; }
    public User FirstConversationalist { get; set; }
    public string SecondConversationalistId { get; set; }
    public User SecondConversationalist { get; set; }

    public List<Message> Messages { get; set; }
}