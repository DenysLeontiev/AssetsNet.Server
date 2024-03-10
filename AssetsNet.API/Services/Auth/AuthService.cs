using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailService _emailService;

    public AuthService(SignInManager<User> signInManager ,UserManager<User> userManager, ITokenHandler tokenHandler,
        IEmailService emailService)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _emailService = emailService;
        _signInManager = signInManager;
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

    public async Task<UserJwtDto> LoginAsync(LoginUserDto loginUserDto)
    {
        var userFromDb = await _userManager.FindByNameAsync(loginUserDto.UserName);

        if (userFromDb == null)
        {
            throw new Exception("This user wasn`t found in database");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, loginUserDto.Password, false);

        if (!result.Succeeded)
        {
            throw new Exception("The password is incorrect");
        }
        
        return new UserJwtDto
        {
            UserName = userFromDb.UserName,
            Email = userFromDb.Email,
            Token = _tokenHandler.CreateToken(userFromDb)
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