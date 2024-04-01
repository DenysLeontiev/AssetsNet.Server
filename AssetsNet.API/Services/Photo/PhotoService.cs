using AssetsNet.API.Helpers.Cloudinary;
using AssetsNet.API.Interfaces.Photo;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace AssetsNet.API.Services.Photo;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinaryCredentials> options)
    {
        var account = new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret);
        
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<Entities.Photo> UploadAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "AssetsNet",
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        var photo = new Entities.Photo
        {
            PublicId = uploadResult.PublicId,
            PhotoUrl = uploadResult.SecureUrl.AbsoluteUri,
        };

        return photo;
    }

    public async Task<DeletionResult> DeleteAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
        return deletionResult;
    }
}