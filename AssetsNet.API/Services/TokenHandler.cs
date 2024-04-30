using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AssetsNet.API.Entities;
using AssetsNet.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AssetsNet.API.Services;

public class TokenHandler : ITokenHandler
{
    private readonly SymmetricSecurityKey _key;
    public TokenHandler(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }


    public string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
            };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}