namespace AssetsNet.API.Helpers.User;

public class SearchedUser
{
    public string UserName { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string? ProfilePhotoUrl { get; set; }

    public SearchedUser(string userName, string id, string? profilePhotoUrl = null)
    {
        UserName = userName;
        Id = id;
        ProfilePhotoUrl = profilePhotoUrl;
    }
}
