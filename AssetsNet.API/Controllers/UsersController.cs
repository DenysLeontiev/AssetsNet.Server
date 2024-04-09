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
    public async Task<ActionResult<PhotoDto>> UploadProfilePhoto([FromForm] UploadProfilePhotoDto uploadProfilePhotoDto)
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var uploadedPhoto = await _userRepository.UploadProfilePhotoAsync(uploadProfilePhotoDto.ProfilePhoto, userId);

            var photoDto = new PhotoDto
            {
                Id = uploadedPhoto.Id!,
                PublicId = uploadedPhoto.PublicId,
                PhotoUrl = uploadedPhoto.PhotoUrl,
                UserId = uploadedPhoto.UserId!
            };

            return Ok(photoDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("followings/{userId}")]
    public async Task<IActionResult> GetUserFollowings(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var followings = await _userRepository.GetUserFollowingsAsync(userId);

            return Ok(followings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("followers/{userId}")]
    public async Task<IActionResult> GetUserFollowers(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var followers = await _userRepository.GetUserFollowersAsync(userId);

            return Ok(followers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("follow-user/{userIdToFollow}")]
    public async Task<IActionResult> FollowUser(string userIdToFollow)
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userIdToFollow))
            {
                return BadRequest("FollowerId and UserId are required.");
            }

            await _userRepository.FollowUserAsync(userId, userIdToFollow);

            return Ok($"User with ID {userId} is now following user with ID {userIdToFollow}.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}