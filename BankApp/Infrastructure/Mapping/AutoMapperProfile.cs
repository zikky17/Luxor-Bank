using AutoMapper;
using BankApp.ViewModels;
using ServiceLibrary.Data;

namespace BankApp.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerViewModel, Customer>()

            .ForMember(dest => dest.Givenname, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();
        }

    }
}
