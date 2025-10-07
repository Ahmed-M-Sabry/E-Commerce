using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        private readonly IProductService _productService;
        private readonly IIdentityServies _identityServies;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductService productService,
                                    IIdentityServies identityServies,
                                    IMapper mapper)
        {
            _productService = productService;
            _identityServies = identityServies;
            _mapper = mapper;
        }

        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = _identityServies.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Result<Product>.Failure("User must be authenticated to create a post.", ErrorType.Unauthorized);

            var newProduct = _mapper.Map<Product>(request);
            newProduct.SellerId = userId;

            var result = await _productService.CreateProductAsync(newProduct, request.PhotosFiles);

            return result;
        }
    }
}
