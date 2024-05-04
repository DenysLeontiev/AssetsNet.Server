using System.Security.Claims;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using AssetsNet.API.DTOs.User;
using AutoMapper;
using ChatGPT.Net.DTO.ChatGPT;
using AssetsNet.API.Helpers;
using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Helpers.User;
using AssetsNet.API.Data;
using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("upload-profile-photo")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    [HttpPut("update-user-requests-limit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateUserRequestsLimit([FromBody] UpdateUserRequestsLimitDto updateUserRequestsLimitDto)
    {
        try
        {
            string userId = User.GetCurrentUserId();

            await _userRepository.UpdateUserRequestsLimitAsync(
                updateUserRequestsLimitDto.TariffPlan,
                updateUserRequestsLimitDto.PaymentState,
                userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("followings/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDto>>> GetUserFollowings(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var followings = await _userRepository.GetUserFollowingsAsync(userId);

            var mappedFollowings = _mapper.Map<List<UserDto>>(followings);

            return Ok(mappedFollowings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("followers/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDto>>> GetUserFollowers(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var followers = await _userRepository.GetUserFollowersAsync(userId);

            var mappedFollowers = _mapper.Map<List<UserDto>>(followers);

            return Ok(mappedFollowers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("follow-user/{userIdToFollow}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> GetUserById(string userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("conversations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversations()
    {
        var convs = await _userRepository.GetConversationsByIdAsync(User.GetCurrentUserId());

        var convsDto = _mapper.Map<IEnumerable<MessageDto>>(convs);

        return Ok(convsDto);
    }
    
    [HttpGet("user-search/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SearchedUser>>> SearchUsersByUserName(string username)
    {
        var users = await _userRepository.SearchUsersByUsernameAsync(username);

        return Ok(users);
    }
}