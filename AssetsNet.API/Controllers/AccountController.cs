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

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenHandler _tokenHandler;

    public AccountController(
        IAuthService authService,
        ILogger<AccountController> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenHandler tokenHandler)
    {
        _authService = authService;
        _logger = logger;

        _userManager = userManager; 
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
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
        User userFromDb = await _userManager.FindByNameAsync(loginUserDto.UserName);
        if (userFromDb == null)
        {
            return BadRequest("This user wasn`t found in database");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, loginUserDto.Password, false);
        if (!result.Succeeded)
        {
            return BadRequest("The password is incorrect");
        }

        return Ok(new UserJwtDto
        {
            UserName = userFromDb.UserName,
            Email = userFromDb.Email,
            Token = _tokenHandler.CreateToken(userFromDb)
        }) ;

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