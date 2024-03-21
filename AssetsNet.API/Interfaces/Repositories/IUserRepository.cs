using AssetsNet.API.DTOs.Photo;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Entities.Photo> UploadProfilePhotoDto(UploadProfilePhotoDto uploadProfilePhotoDto);
}