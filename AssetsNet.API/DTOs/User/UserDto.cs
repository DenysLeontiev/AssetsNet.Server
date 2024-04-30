namespace AssetsNet.API.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public int NumberOfFollowers { get; set; }
        public int NumberOfFollowings{ get; set; }
    }

}
