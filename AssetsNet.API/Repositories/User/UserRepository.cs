using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers;
using AssetsNet.API.Helpers.User;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AssetsNet.API.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AssetsDbContext _context;
    private readonly IPhotoService _photoService;

    public UserRepository(AssetsDbContext context,
        IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    public async Task UpdateUserRequestsLimitAsync(TariffPlansEnum tariff, int paymentState, string userId)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId))
            ?? throw new Exception("User is not found");

        if (paymentState is not TariffPlanConsts.SuccessPaymentState)
        {
            throw new Exception("Payment state is not success");
        }

        if (userToUpdate.GptRequestsLeft > 0)
        {
            throw new Exception("User has enought Gpt requests");
        }
        else
        {
            userToUpdate.GptRequestsLeft = tariff switch
            {
                TariffPlansEnum.Basic => TariffPlanConsts.BasicGptRequests,
                TariffPlansEnum.Premium => TariffPlanConsts.PremiumGptRequests,
                _ => throw new Exception("Tariff plan is not found")
            };

            await _context.SaveChangesAsync();
        }
    }

    public async Task<Photo> UploadProfilePhotoAsync(IFormFile file, string userId)
    {
        var userToUpdateProfilePhoto = await _context.Users.Include(x => x.ProfilePhoto)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId)) ?? throw new Exception("User with is not found");


        if (userToUpdateProfilePhoto!.ProfilePhoto is not null)
        {
            var result = await _photoService.DeleteAsync(userToUpdateProfilePhoto.ProfilePhoto.PublicId);

            if (result.Error is not null)
            {
                throw new Exception(result.Error.Message);
            }
        }

        var photo = await _photoService.UploadAsync(file);

        userToUpdateProfilePhoto!.ProfilePhoto = photo;

        await _context.SaveChangesAsync();

        return photo;
    }

    public async Task<Entities.User> FollowUserAsync(string followerId, string userId)
    {
        var followerUser = await _context.Users.FindAsync(followerId)
            ?? throw new Exception("User is not found");

        var userToFollow = await _context.Users.FindAsync(userId)
            ?? throw new Exception("User is not found");

        var existingFollow = await _context.UserFollows.FindAsync(userId, followerId);
        if (existingFollow != null)
        {
            throw new Exception("You are already following this user");
        }

        var follow = new UserFollow
        {
            UserId = userId,
            FollowerId = followerId,
        };

        await _context.UserFollows.AddAsync(follow);
        await _context.SaveChangesAsync();

        return userToFollow;
    }

    public async Task<List<Entities.User>> GetUserFollowingsAsync(string userId)
    {
        var followings = await _context.UserFollows
            .Where(uf => uf.FollowerId == userId)
            .Include(x => x.Follower.ProfilePhoto)
            .Select(uf => uf.User)
            .ToListAsync();

        return followings;
    }

    public async Task<List<Entities.User>> GetUserFollowersAsync(string userId)
    {
        var followers = await _context.UserFollows
            .Where(uf => uf.UserId == userId)
            .Include(x => x.Follower.ProfilePhoto)
            .Select(uf => uf.Follower)
            .ToListAsync();

        return followers;
    }
    public async Task<Entities.User> GetUserByIdAsync(string userId)
    {
        var user = await _context.Users
            .Include(x => x.ProfilePhoto)
            .Include(x => x.Followers)
            .Include(x => x.Following)
                    .FirstOrDefaultAsync(x => x.Id.Equals(userId));

        return user;
    }

    public async Task<IEnumerable<Entities.Message>> GetConversationsByIdAsync(string userId)
    {
        var messages = await _context.Messages
            .Include(m => m.Recipient)
                .ThenInclude(u => u!.ProfilePhoto)
            .Include(m => m.Sender)
                .ThenInclude(u => u!.ProfilePhoto)
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .GroupBy(m => new
            {
                MinId = m.SenderId.CompareTo(m.RecipientId) < 0 ? m.SenderId : m.RecipientId,
                MaxId = m.SenderId.CompareTo(m.RecipientId) < 0 ? m.RecipientId : m.SenderId
            })
            .Select(g => g.OrderByDescending(m => m.DateSent).FirstOrDefault())
            .ToListAsync();

        return messages!;
    }

    public async Task<List<SearchedUser>> SearchUsersByUsernameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return null;
        }

        int maxAmountOfUsers = 30;
        return await _context.Users.Include(p => p.ProfilePhoto)
                                   .Where(x => x.NormalizedUserName.Contains(username.ToUpper()))
                                   .Select(x => new SearchedUser(x.UserName, x.Id, x.ProfilePhoto.PhotoUrl))
                                   .AsSplitQuery()
                                   .Take(maxAmountOfUsers)
                                   .ToListAsync();
    }

    public async Task<List<string>> GetUserFollowersUserName(string userId)
    {
        return await _context.UserFollows.Where(x => x.FollowerId.Equals(userId)).Select(x => x.User.UserName).ToListAsync();
    }
}