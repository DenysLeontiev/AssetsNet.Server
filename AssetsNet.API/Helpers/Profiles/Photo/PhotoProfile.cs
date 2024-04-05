using AssetsNet.API.DTOs.Photo;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles.Photo;

public class PhotoProfile : Profile
{
    public PhotoProfile()
    {
        CreateMap<Entities.Photo, PhotoDto>();
    }
}