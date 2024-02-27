using AssetsNet.API.Entities;

namespace AssetsNet.API.Interfaces;

public interface ITokenHandler
{
    string CreateToken(User user, IList<string> roles);
}