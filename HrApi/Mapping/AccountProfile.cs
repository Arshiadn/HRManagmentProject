using AutoMapper;
using HrApi.Models;
using HrApi.ViewModels;

namespace HrApi.Mapping;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        //Register
        CreateMap<RegisterViewModel, ApplicationUser>()
                    .ForMember(dest => 
                    dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
    }
}
