using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.User;

namespace AssetsNet.API.Interfaces.Auth;

public interface IAuthService
{
    Task<UserJwtDto> RegisterAsync(RegisterUserDto registerUserDto);
}