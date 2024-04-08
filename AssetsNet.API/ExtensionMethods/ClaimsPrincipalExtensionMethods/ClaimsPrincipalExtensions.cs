using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;

public static class ClaimsPrincipalExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValueByClaimName(claimsPrincipal, ClaimTypes.NameIdentifier);
    }

    public static string GetCurrentUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValueByClaimName(claimsPrincipal, ClaimTypes.Name);
    }

    public static string GetCurrentUserEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValueByClaimName(claimsPrincipal, ClaimTypes.Email);
    }

    private static string GetClaimValueByClaimName(ClaimsPrincipal claimsPrincipal, string claimName)
    {
        var claim = claimsPrincipal.FindFirst(claimName);

        if (claim is not null)
        {
            return claim.Value;
        }

        throw new InvalidOperationException($"{claimName} cannot be found");
    }
}