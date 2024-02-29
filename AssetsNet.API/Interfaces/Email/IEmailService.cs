using AssetsNet.API.DTOs.Email;

namespace AssetsNet.API.Interfaces.Email;
public interface IEmailService 
{
    Task<bool> SendEmailAsync(EmailSendDto emailSendDto);        
}