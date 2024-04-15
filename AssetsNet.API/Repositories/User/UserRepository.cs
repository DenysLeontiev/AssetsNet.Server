﻿using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;
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
    //TODO: null response
    public async Task<Entities.User> GetUser(string userId)
    {
        var user = await _context.Users
            .Include(x => x.ProfilePhoto)
            .Include(x => x.Followers)
            .Include(x => x.Following)
                    .FirstOrDefaultAsync(x => x.Id.Equals(userId));

        return user;
    }
}