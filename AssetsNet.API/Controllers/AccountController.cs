using AssetsNet.API.DTOs;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;

    public AccountController(UserManager<User> userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto)
    {
        var userToCreate = new User
        {
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
        };

        var result = await _userManager.CreateAsync(userToCreate, registerUserDto.Password);

        if(result.Succeeded)
        {
            return Ok(new {
                UserName = userToCreate.UserName,
                Email = userToCreate.Email,
                Token = _tokenHandler.CreateToken(userToCreate)
            });
        }

        return BadRequest(result);
    }
}