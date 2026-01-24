using AutoMapper;
using Reservei.Api.DTOs;
using Reservei.Api.Entities;

namespace Reservei.Api.Mappings;

public class ProfessionalProfile : Profile
{
    public ProfessionalProfile()
    {
        CreateMap<Professional, ProfessionalResponseDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        CreateMap<CreateProfessionalDto, Professional>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
