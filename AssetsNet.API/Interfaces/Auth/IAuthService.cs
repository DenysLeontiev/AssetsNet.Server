using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;

namespace AssetsNet.API.Interfaces.Auth;

public interface IAuthService
{
    Task<UserJwtDto> LoginAsync(LoginUserDto loginUserDto);
    Task<UserJwtDto> RegisterAsync(RegisterUserDto registerUserDto);
    Task<UserJwtDto> LoginWithGoogleAsync(string credentials);
    Task ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto);
}