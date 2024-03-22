using AssetsNet.API.DTOs.Photo;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Entities.Photo> UploadProfilePhotoAsync(IFormFile file, string userId);
}