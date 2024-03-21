using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<Photo> UploadProfilePhotoDto(UploadProfilePhotoDto uploadProfilePhotoDto)
    {
        var userToUpdateProfilePhoto = await _context.Users.Include(x => x.ProfilePhoto)
            .FirstOrDefaultAsync(x => x.Id.Equals(uploadProfilePhotoDto.UserId));

        var photo = await _photoService.UploadAsync(uploadProfilePhotoDto.ProfilePhoto);

        userToUpdateProfilePhoto!.ProfilePhoto = photo;

        await _context.SaveChangesAsync();

        return photo;
    }
}