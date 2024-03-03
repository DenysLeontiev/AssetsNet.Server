namespace AssetsNet.API.DTOs.Email;
public class EmailConfirmationDto
{
    public string Email { get; set; } = string.Empty;

    public int VerificationCode { get; set; }
}