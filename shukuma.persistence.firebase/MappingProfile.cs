using AutoMapper;
using shukuma.domain.Models;

namespace shukuma.persistence.firebase;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // UserEntity <-> UserModel (bidirectional)
        CreateMap<UserModel, UserEntity>()
            .ReverseMap();

        // UserEntity <-> UserInfo (bidirectional)
        CreateMap<UserInfo, UserEntity>()
            .ReverseMap();

        // UserInfo -> UserModel (NEW - this was missing!)
        CreateMap<UserInfo, UserModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
            .ForMember(dest => dest.CardsCompleted, opt => opt.MapFrom(src => src.CardsCompleted))
            .ForMember(dest => dest.TimeCompleted, opt => opt.MapFrom(src => src.TimeCompleted))
            .ForMember(dest => dest.CompletedBy, opt => opt.MapFrom(src => src.CompletedBy))
            .ForMember(dest => dest.Review, opt => opt.MapFrom(src => src.Review));

        // SignupModel -> UserEntity
        CreateMap<SignupModel, UserEntity>();

        // SignupModel -> UserModel
        CreateMap<SignupModel, UserModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}