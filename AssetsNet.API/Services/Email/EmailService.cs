using AssetsNet.API.DTOs.Email;
using AssetsNet.API.Interfaces.Email;
using AssetsNet.API.Models.Email;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;

namespace AssetsNet.API.Services.Email;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IOptions<EmailSettings> options,
        IConfiguration configuration)
    {
        Options = options.Value;
        _configuration = configuration;
    }

    public EmailSettings Options { get; set; }

    public async Task<bool> SendEmailAsync(EmailSendDto emailSendDto)
    {
        MailjetClient client = new MailjetClient(_configuration["MailJet:ApiKey"], _configuration["MailJet:SecretKey"]);

        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact(Options.From, Options.ApplicationName))
            .WithSubject(emailSendDto.Subject)
            .WithHtmlPart(emailSendDto.Body)
            .WithTo(new SendContact(emailSendDto.To))
            .Build();

        var response = await client.SendTransactionalEmailAsync(email);

        if (response.Messages is not null)
        {
            return response.Messages.First().Status == "success";
        }

        return false;
    }
}