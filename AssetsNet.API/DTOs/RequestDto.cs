namespace AssetsNet.API.DTOs;

public class RequestDto
{
    public string Id { get; set; }
    public string RequestToAI { get; set; }
    public string ResponseFromAI { get; set; }
    public DateTime RequestedAt { get; set; }
}