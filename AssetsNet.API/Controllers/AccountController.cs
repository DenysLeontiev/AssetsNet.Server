using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Email;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.Email;
using AssetsNet.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthService authService,
        ILogger<AccountController> logger)
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
    //Dependency injection, Модульність та розділення відповідальності
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto) 
    {
        try 
        {
            var result = await _authService.LoginAsync(loginUserDto);
            _logger.LogInformation($"Login to account was successful");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "An error occured while login");
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConfirmEmail([FromBody] EmailConfirmationDto emailConfirmationDto)
    {
        try
        {
            await _authService.ConfirmEmailAsync(emailConfirmationDto);
            _logger.LogInformation("User {Email} confirmed email successfully.", emailConfirmationDto.Email);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "An error occured while confirming email");
            return BadRequest(ex.Message);
        }
    }
}