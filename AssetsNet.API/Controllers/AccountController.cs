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
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserJwtDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserJwtDto>> Login([FromBody] LoginUserDto loginUserDto) 
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

    [HttpPost("google-account")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserJwtDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<UserJwtDto>> GoogleAccount([FromBody] string credential)
    {
        try
        {
			var userJwt = await _authService.LoginWithGoogleAsync(credential);
            _logger.LogInformation("User {UserName} logged in successfully.", userJwt.UserName);

			return Created("", userJwt);
		}
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "User login failed");
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