namespace AssetsNet.API.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePhotoUrl { get; set; } = string.Empty;
        public int NumberOfFollowers { get; set; }
        public int NumberOfFollowings{ get; set; }
    }
}
