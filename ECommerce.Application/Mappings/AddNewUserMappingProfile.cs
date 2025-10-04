using AutoMapper;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Model;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Mappings
{
    public class AddNewUserMappingProfile : Profile
    {
        public AddNewUserMappingProfile()
        {
            CreateMap<AddBuyerUserCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.StoreName, opt => opt.Ignore())
                .ForMember(dest => dest.StoreAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 

            CreateMap<AddSellerUserCommand,ApplicationUser>()
                .ForMember(dest=>dest.PhoneNumber , opt=>opt.MapFrom(src=>src.PhoneNumber))
                .ForMember(dest=>dest.Address , opt=>opt.MapFrom(src=>src.Address))
                .ForMember(dest=>dest.StoreAddress , opt=>opt.MapFrom(src=>src.StoreAddress))
                .ForMember(dest=>dest.StoreName , opt=>opt.MapFrom(src=>src.StoreName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 
        }
    }
}
