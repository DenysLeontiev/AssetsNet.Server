using System.Security.Claims;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("followings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFollowings()
    {
        try
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var userFollowings = await _userRepository.GetUserFollowings(currentUserId);

            _logger.LogInformation("Successfully retrieved followings for User ({userId})", currentUserId);

            return Ok(userFollowings);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error happened during retrieving followings for user", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("followers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFollowers()
    {
        try
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var userFollowers = await _userRepository.GetUserFollowers(currentUserId);

            _logger.LogInformation("Successfully retrieved followers for User ({userId})", currentUserId);

            return Ok(userFollowers);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error happened during retrieving followers for user", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("upload-profile-photo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PhotoDto>> UploadProfilePhoto([FromForm] UploadProfilePhotoDto uploadProfilePhotoDto)
    {
        try
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var uploadedPhoto = await _userRepository.UploadProfilePhotoAsync(uploadProfilePhotoDto.ProfilePhoto, currentUserId);

            var photoDto = new PhotoDto
            {
                Id = uploadedPhoto.Id!,
                PublicId = uploadedPhoto.PublicId,
                PhotoUrl = uploadedPhoto.PhotoUrl,
                UserId = uploadedPhoto.UserId!
            };

            _logger.LogInformation("Successfully uploaded photofor User ({userId})", currentUserId);

            return Ok(photoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error happened during photo upload for user", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("follow-user/{userIdToFollow}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FollowUser(string userIdToFollow)
    {
        try
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var followedUser = await _userRepository.FollowUser(currentUserId, userIdToFollow);

            _logger.LogInformation("User({currentUserId}) successfully followed user({followedUserId})", currentUserId, userIdToFollow);

            return Ok(followedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError("Can`t follow this user", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}