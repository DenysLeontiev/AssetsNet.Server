
using AssetsNet.API.DTOs;
using AssetsNet.API.Entities;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<Request, RequestDto>();
    }
}