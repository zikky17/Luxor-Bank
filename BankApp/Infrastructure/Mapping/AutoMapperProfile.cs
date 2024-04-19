using AutoMapper;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using ServiceLibrary.Data;
using ServiceLibrary.ViewModels;

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

            CreateMap<AccountViewModel, Account>()
                .ReverseMap();

            CreateMap<TransactionViewModel, Transaction>()
                .ReverseMap();

            CreateMap<UserViewModel, IdentityUser>()
                .ReverseMap();
        }

    }
}
