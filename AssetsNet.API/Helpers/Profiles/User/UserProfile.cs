using AssetsNet.API.DTOs.DatabaseDTO;
using AssetsNet.API.Entities;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}