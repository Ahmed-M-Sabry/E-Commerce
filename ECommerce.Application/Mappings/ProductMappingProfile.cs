using AutoMapper;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.EditProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetAllProductByPagination;
using ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetProductById;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.SellerId, opt => opt.Ignore());

            CreateMap<EditProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.SellerId, opt => opt.Ignore());

            CreateMap<Product, GetAllProductByPaginationDto>()
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.PhotosName,
                        opt => opt.MapFrom(src => src.Photos.Select(p => p.ImageName).ToList()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.rating))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<Product, GetProductByIdDto>()
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.PhotosName,
                        opt => opt.MapFrom(src => src.Photos.Select(p => p.ImageName).ToList()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.rating))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.FullName))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));
        }
    }
}
