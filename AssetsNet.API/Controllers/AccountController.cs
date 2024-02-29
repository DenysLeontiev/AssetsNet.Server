using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.Email;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthService authService, ILogger<AccountController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserJwtDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserJwtDto>> Register([FromBody] RegisterUserDto registerUserDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerUserDto);
            _logger.LogInformation("User {UserName} registered successfully.", result.UserName);

            return Created("", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "An error occured while registreing new User");
            return BadRequest(ex.Message);
        }
    }
}