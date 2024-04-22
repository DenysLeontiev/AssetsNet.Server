using AssetsNet.API.DTOs.Message;
using AssetsNet.API.Entities;
using AutoMapper;

namespace AssetsNet.API.Helpers.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, src => src.MapFrom(opts => opts.Sender.UserName))
                .ForMember(dest => dest.RecipientName, src => src.MapFrom(opts => opts.Recipient.UserName))
                .ForMember(dest => dest.RecipientPhotoUrl, src => src.MapFrom(opts => opts.Recipient.ProfilePhoto!.PhotoUrl));
    }
}