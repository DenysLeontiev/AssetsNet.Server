namespace AssetsNet.API.DTOs.Photo;

public class UploadProfilePhotoDto
{
    public string UserId { get; set; } = string.Empty;
    public IFormFile ProfilePhoto { get; set; }
}