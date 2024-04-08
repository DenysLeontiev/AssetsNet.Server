using AssetsNet.API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace AssetsNet.API.Entities;

public class User : IdentityUser
{
    public User()
    {
        GptRequestsLeft = TariffPlanConsts.FreeGptRequests;
    }

    public int VerificationCode { get; set; }
    public int GptRequestsLeft { get; set; }
    public Photo? ProfilePhoto { get; set; }
}