using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Entities;

public class User : IdentityUser
{
    public int VerificationCode { get; set; }
    public Photo? ProfilePhoto { get; set; }
}