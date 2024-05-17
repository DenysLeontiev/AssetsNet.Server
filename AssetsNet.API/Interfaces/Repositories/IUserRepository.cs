using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Helpers;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers.User;
using AssetsNet.API.DTOs.User;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Entities.Photo> UploadProfilePhotoAsync(IFormFile file, string userId);
    Task UpdateUserRequestsLimitAsync(TariffPlansEnum tariff, int paymentState, string userId);
    Task<User> FollowUserAsync(string followerId, string userId);
    Task<IEnumerable<Message>> GetConversationsByIdAsync(string userId);
    Task<List<User>> GetUserFollowingsAsync(string userId);
    Task<List<User>> GetUserFollowersAsync(string userId);
    Task<List<SearchedUser>> SearchUsersByUsernameAsync(string username);
    Task<Entities.User> GetUserByIdAsync(string userId);
    Task<List<string>> GetUserFollowersUserName(string userId);
    Task<Entities.User> UpdateUserInfoAsync(UpdateUserInfoDto userInfo, string userId);
}