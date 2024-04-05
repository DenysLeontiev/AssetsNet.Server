using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;

public static class ClaimsPrincipalExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValueByClaimName(claimsPrincipal, JwtRegisteredClaimNames.NameId);
    }

    private static string GetClaimValueByClaimName(ClaimsPrincipal claimsPrincipal, string claimName)
    {
        var claim = claimsPrincipal.FindFirst(claimName);

        if(claim != null)
        {
            return claim.Value;
        }

        throw new InvalidOperationException($"{claimName} cannot be found");
    }
}