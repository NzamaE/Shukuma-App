using AutoMapper;
using shukuma.domain.Models;
using shukuma.persistence.firebase;

namespace shukuma.application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserModel, UserEntity>()
            .ReverseMap();

        CreateMap<SignupModel, UserEntity>();
        CreateMap<SignupModel, UserModel>();
    }
}
