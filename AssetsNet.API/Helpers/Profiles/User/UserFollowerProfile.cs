using AssetsNet.API.DTOs.DatabaseDTOs;
using AssetsNet.API.Entities;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class UserFollowerProfile : Profile
{
    public UserFollowerProfile()
    {
        CreateMap<UserFollower, UserFollowerDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Follower.Id))
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Follower.UserName))
            .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Follower.Email))
            .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(src => src.Follower.ProfilePhoto != null ? src.Follower.ProfilePhoto.PhotoUrl : string.Empty));
    }
}