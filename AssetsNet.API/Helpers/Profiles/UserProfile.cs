using AssetsNet.API.DTOs.User;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Entities.User, UserDto>()
            .ForMember(dest => dest.ProfilePhotoUrl,
                       src => src.MapFrom(opts => opts.ProfilePhoto.PhotoUrl))
            .ForMember(dest => dest.NumberOfFollowers, src => src.MapFrom(opts => opts.Followers.Count))
            .ForMember(dest => dest.NumberOfFollowings, src => src.MapFrom(opts => opts.Following.Count));

    }
}
