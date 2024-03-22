using System.Security.Claims;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("upload-profile-photo")]
    public async Task<ActionResult> UploadProfilePhoto([FromForm] UploadProfilePhotoDto uploadProfilePhotoDto)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var uploadedPhoto = await _userRepository.UploadProfilePhotoDto(uploadProfilePhotoDto);

        return Ok(uploadedPhoto);
    }
}