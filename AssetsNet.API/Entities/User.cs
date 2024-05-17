using AssetsNet.API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Entities;

public class User : IdentityUser
{
    public User()
    {
        GptRequestsLeft = TariffPlanConsts.FreeGptRequests;
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public int VerificationCode { get; set; }
    public int GptRequestsLeft { get; set; }
    public Photo? ProfilePhoto { get; set; }
    public virtual ICollection<UserFollow> Followers { get; set; }
    public virtual ICollection<UserFollow> Following { get; set; }
    public List<Message> MessagesSent { get; set; }
    public List<Message> MessagesRecieved { get; set; }
}