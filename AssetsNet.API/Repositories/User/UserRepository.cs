﻿using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers;
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

    public async Task FollowUserAsync(string followerId, string userId)
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
    }

    public async Task<List<Entities.User>> GetUserFollowingsAsync(string userId)
    {
        var followings = await _context.UserFollows
            .Where(uf => uf.FollowerId == userId)
            .Select(uf => uf.User)
            .ToListAsync();

        return followings;
    }

    public async Task<List<Entities.User>> GetUserFollowersAsync(string userId)
    {
        var followers = await _context.UserFollows
            .Where(uf => uf.UserId == userId)
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

    public async Task<List<Conversation>> GetConversationsByIdAsync(string userId)
    {
        var user = await _context.Users.Include(x => x.MessagesRecieved)
                                       .Include(x => x.MessagesSent)
                                       .Include(x => x.ProfilePhoto)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id.Equals(userId));
        if (user == null)
        {
            return new List<Conversation>();
        }

        List<Conversation> conversations = new List<Conversation>();

        // Get unique recipient IDs from sent messages
        var uniqueRecipientsSent = user.MessagesSent.Select(m => m.RecipientId).Distinct().ToList();

        // Add conversations where user sent messages
        foreach (var recipientId in uniqueRecipientsSent)
        {
            var recipient = await _context.Users.Include(x => x.ProfilePhoto).FirstOrDefaultAsync(x => x.Id.Equals(recipientId));
            if (recipient != null)
            {
                conversations.Add(new Conversation
                {
                    SenderName = user.UserName,
                    SenderId = user.Id,
                    RecipientName = recipient.UserName,
                    RecipientId = recipient.Id
                });
            }
        }

        // Get unique sender IDs from received messages
        var uniqueSendersReceived = user.MessagesRecieved.Select(m => m.SenderId).Distinct().ToList();

        // Add conversations where user received messages
        foreach (var senderId in uniqueSendersReceived)
        {
            var sender = await _context.Users.Include(x => x.ProfilePhoto).FirstOrDefaultAsync(x => x.Id.Equals(senderId));
            if (sender != null)
            {
                conversations.Add(new Conversation
                {
                    SenderName = sender.UserName,
                    SenderId = sender.Id,
                    RecipientName = user.UserName,
                    RecipientId = user.Id
                });
            }
        }

        return conversations;
    }
}