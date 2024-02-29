namespace AssetsNet.API.DTOs.Email;
public class EmailSendDto
{
    public EmailSendDto(string to, string subject, string body)
    {
        To = to;
        Subject = subject;
        Body = body;
    }

    public string To { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}