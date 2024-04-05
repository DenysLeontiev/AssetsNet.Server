using AssetsNet.API.DTOs.DatabaseDTOs;
using AssetsNet.API.Entities;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class UserFollowingProfile : Profile
{
    public UserFollowingProfile()
    {
        CreateMap<UserFollowing, UserFollowingDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Following.Id))
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Following.UserName))
            .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Following.Email))
            .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(src => src.Following.ProfilePhoto != null ? src.Following.ProfilePhoto.PhotoUrl : string.Empty));
    }
}