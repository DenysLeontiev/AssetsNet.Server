using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.Email;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, ITokenHandler tokenHandler,
        IEmailService emailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _emailService = emailService;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<UserJwtDto> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var userToCreate = new User
        {
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
        };

        var result = await _userManager.CreateAsync(userToCreate, registerUserDto.Password);

        if (!result.Succeeded)
        {
            throw new Exception("User creation failed! Errors: " + string.Join(" ", result.Errors));
        }

        await SendVerificationEmail(userToCreate);

        return new UserJwtDto
        {
            Id = userToCreate.Id,
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
            throw new Exception("Login or passsword is incorrect");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, loginUserDto.Password, false);

        if (!result.Succeeded)
        {
            throw new Exception("Login or passsword is incorrect");
        }

        return new UserJwtDto
        {
            Id = userFromDb.Id,
            UserName = userFromDb.UserName,
            Email = userFromDb.Email,
            Token = _tokenHandler.CreateToken(userFromDb)
        };
    }

    public async Task<UserJwtDto> LoginWithGoogleAsync(string credential)
    {
        string clientId = _configuration.GetSection("GoogleClientId").Value;
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { clientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.Equals(payload.Email));

        UserJwtDto userJwtDto = new UserJwtDto();

        if (user == null)
        {
            var userToCreate = new User
            {
                UserName = payload.Name,
                Email = payload.Email,
                EmailConfirmed = payload.EmailVerified,
                NormalizedEmail = payload.Email.ToUpper(),
                NormalizedUserName = payload.Name.ToUpper(),
            };

            var result = await _userManager.CreateAsync(userToCreate);

            if (!result.Succeeded)
            {
                throw new Exception("Google Authentification failed! Errors: " + string.Join(" ", result.ToString()));
            }

            await _userManager.AddToRoleAsync(userToCreate, DbRolesConsts.MemberRole);

            userJwtDto = GetUserJwtDto(userToCreate);

            return userJwtDto;
        }
        else
        {
            userJwtDto = GetUserJwtDto(user);
            return userJwtDto;
        }
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

    private UserJwtDto GetUserJwtDto(User user)
    {
        return new UserJwtDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Id = user.Id,
            Token = _tokenHandler.CreateToken(user)
        };
    }
}