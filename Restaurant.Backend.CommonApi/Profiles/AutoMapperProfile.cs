using AutoMapper;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;

namespace Restaurant.Backend.CommonApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();

            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.IdentificationTypeId, act => act.MapFrom(src => src.IdentificationTypeId))
                .ForMember(dest => dest.IdentificationType, act => act.Ignore());
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.IdentificationTypeId, act => act.MapFrom(src => src.IdentificationType.Id))
                .ForMember(dest => dest.IdentificationType, act => act.MapFrom(src => src.IdentificationType.Name));

            CreateMap<IdentificationTypeDto, IdentificationType>();
            CreateMap<IdentificationType, IdentificationTypeDto>();
        }
    }
}
