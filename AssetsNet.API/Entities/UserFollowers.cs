namespace AssetsNet.API.Entities;

public class UserFollowers
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; }
    public string FollowerId { get; set; } = string.Empty;
    public User Follower { get; set; }
}