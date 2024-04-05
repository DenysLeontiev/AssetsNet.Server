using AssetsNet.API.DTOs.DatabaseDTO;
using AssetsNet.API.DTOs.DatabaseDTOs;
using AssetsNet.API.DTOs.Photo;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<PhotoDto> UploadProfilePhotoAsync(IFormFile file, string userId);
    Task<UserDto> FollowUser(string currentUserId,string userIdToFollow);
    Task<List<UserFollowingDto>> GetUserFollowings(string userId);
    Task<List<UserFollowerDto>> GetUserFollowers(string userId);
}