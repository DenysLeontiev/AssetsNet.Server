using CloudinaryDotNet.Actions;

namespace AssetsNet.API.Interfaces.Photo;

public interface IPhotoService
{
    Task<Entities.Photo> UploadAsync(IFormFile file);
    Task<DeletionResult> DeleteAsync(string publicId);
}