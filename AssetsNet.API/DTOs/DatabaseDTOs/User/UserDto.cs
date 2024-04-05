using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;

namespace AssetsNet.API.DTOs.DatabaseDTO;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PhotoDto ProfilePhoto { get; set; } = new();
}