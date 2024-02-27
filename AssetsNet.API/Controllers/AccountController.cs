using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.User;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserJwtDto>> RegisterAsync([FromBody] RegisterUserDto registerUserDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerUserDto);
            return Created("", result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}