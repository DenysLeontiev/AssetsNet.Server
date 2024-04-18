using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Entities;

public class User : IdentityUser
{
    public int VerificationCode { get; set; }
    public Photo? ProfilePhoto { get; set; }
    public virtual ICollection<UserFollow> Followers { get; set; }
    public virtual ICollection<UserFollow> Following { get; set; }
    public List<Message> MessagesSent { get; set; }
    public List<Message> MessagesRecieved { get; set; }

    internal static object FindFirst(object nameIdentifier)
    {
        throw new NotImplementedException();
    }
}