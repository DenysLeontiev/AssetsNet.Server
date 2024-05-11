﻿using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Helpers;
using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Entities.Photo> UploadProfilePhotoAsync(IFormFile file, string userId);
    Task UpdateUserRequestsLimitAsync(TariffPlansEnum tariff, int paymentState, string userId);
    Task FollowUserAsync(string followerId, string userId);
    Task<IEnumerable<Message>> GetConversationsByIdAsync(string userId);
    Task<List<User>> GetUserFollowingsAsync(string userId);
    Task<List<User>> GetUserFollowersAsync(string userId);
    Task<Entities.User> GetUserByIdAsync(string userId);
    Task<List<Request>> GetUserRequestsAsync(string userId);
}