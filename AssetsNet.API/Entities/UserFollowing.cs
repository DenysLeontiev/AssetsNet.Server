namespace AssetsNet.API.Entities;

public class UserFollowing
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; }
    public string FollowingId { get; set; } = string.Empty;
    public User Following { get; set; }
}