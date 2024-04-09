namespace AssetsNet.API.Entities;

public class UserFollow
{
    public string UserId { get; set; }
    public User User { get; set; } // User who is being followed

    public string FollowerId { get; set; }
    public User Follower { get; set; } // User who is following
}