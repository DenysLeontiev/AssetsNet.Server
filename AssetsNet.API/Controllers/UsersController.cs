using System.Security.Claims;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;
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
}