using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.Email;
using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<User> userManager, ITokenHandler tokenHandler,
        IEmailService emailService)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _emailService = emailService;
    }

    public async Task<UserJwtDto> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var userToCreate = new User
        {
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
        };

        var result = await _userManager.CreateAsync(userToCreate, registerUserDto.Password);

        await SendVerificationEmail(userToCreate);

        if (!result.Succeeded)
        {
            throw new Exception("User creation failed! Errors: " + string.Join(" ", result.Errors));
        }

        return new UserJwtDto
        {
            UserName = userToCreate.UserName,
            Email = userToCreate.Email,
            Token = _tokenHandler.CreateToken(userToCreate)
        };
    }

    public async Task ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto)
    {
        var user = await _userManager.FindByEmailAsync(emailConfirmationDto.Email);

        if (user is null)
        {
            throw new Exception("User not found!");
        }

        if (user.VerificationCode != emailConfirmationDto.VerificationCode)
        {
            throw new Exception("Invalid verification code!");
        }

        user.EmailConfirmed = true;

        await _userManager.UpdateAsync(user);
    }

    private async Task SendVerificationEmail(User user)
    {
        var verificationCode = new Random().Next(1000, 9999);

        var email = new DTOs.Email.EmailSendDto(user.Email, "Email Verification",
            $"Please enter the following code to verify your email: {verificationCode}");
        
        user.VerificationCode = verificationCode;

        await _userManager.UpdateAsync(user);

        await _emailService.SendEmailAsync(email);
    }
}