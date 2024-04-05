using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;

public static class ClaimsPrincipalExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claimId = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.NameId);
        if(claimId != null)
        {
            return claimId.Value;
        }

        throw new InvalidOperationException("NameId cannot be found");
    }
}