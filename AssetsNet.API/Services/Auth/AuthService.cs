using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<User> userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
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

        return new UserJwtDto
        {
            UserName = userToCreate.UserName,
            Email = userToCreate.Email,
            Token = _tokenHandler.CreateToken(userToCreate)
        };
    }
}