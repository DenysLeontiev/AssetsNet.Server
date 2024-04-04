using AssetsNet.API.Data;
using AssetsNet.API.DTOs.DatabaseDTO;
using AssetsNet.API.DTOs.DatabaseDTOs;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AssetsDbContext _context;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public UserRepository(AssetsDbContext context,
        IPhotoService photoService,
        IMapper mapper)
    {
        _context = context;
        _photoService = photoService;
        _mapper = mapper;
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

    public async Task<UserDto> FollowUser(string currentUserId, string userIdToFollow)
    {
        var currentUser = await _context.Users.Include(x => x.Followings)
                                              .FirstOrDefaultAsync(x => x.Id.Equals(currentUserId))
                                              ?? throw new Exception($"User ({currentUserId}) is not found");

        var userToFollow = await _context.Users.Include(x => x.Followers)
                                               .FirstOrDefaultAsync(x => x.Id.Equals(userIdToFollow))
                                               ?? throw new Exception($"User ({userIdToFollow}) is not found");

        if (await _context.UserFollowings.AnyAsync(x => x.FollowingId.Equals(userIdToFollow)
            && x.UserId.Equals(currentUserId)))
        {
            throw new Exception("You are already following this user");
        }

        var userFollowing = new UserFollowing
        {
            UserId = currentUserId,
            FollowingId = userIdToFollow
        };

        if (await _context.UserFollowers.AnyAsync(x => x.FollowerId.Equals(userIdToFollow)
            && x.UserId.Equals(currentUserId)))
        {
            throw new Exception("You are already following this user");
        }

        var userFollower = new UserFollower
        {
            UserId = userIdToFollow,
            FollowerId = currentUserId
        };

        _context.UserFollowings.Add(userFollowing);
        _context.UserFollowers.Add(userFollower);

        await _context.SaveChangesAsync();

        var mappedUser = _mapper.Map<UserDto>(userToFollow);

        return mappedUser;
    }

    public async Task<List<UserFollowingDto>> GetUserFollowings(string userId)
    {
        var userFollowings = await _context.UserFollowings
                                       .Include(uf => uf.User)
                                       .Include(uf => uf.Following)
                                       .ThenInclude(p => p.ProfilePhoto)
                                       .Where(uf => uf.UserId == userId)
                                       .ToListAsync();

        var userFollowingsDto = userFollowings.Select(uf => new UserFollowingDto
        {
            UserName = uf.Following.UserName,
            Id = uf.Following.Id,
            PhotoUrl = uf.Following.ProfilePhoto?.PhotoUrl!
        }).ToList();

        return userFollowingsDto;
    }

    public async Task<List<UserFollowerDto>> GetUserFollowers(string userId)
    {
        var userFollowers = await _context.UserFollowers.Include(x => x.Follower).ThenInclude(p => p.ProfilePhoto)
                                                        .Include(x => x.User).Where(x => x.UserId.Equals(userId))
                                                        .ToListAsync();


        var userFollowersDto = userFollowers.Select(uf => new UserFollowerDto
        {
            UserName = uf.Follower.UserName,
            Id = uf.Follower.Id,
            PhotoUrl = uf.Follower.ProfilePhoto?.PhotoUrl!
        }).ToList();

        return userFollowersDto;
    }
}