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

namespace ECommerce.Application.Features.ProductFeatures.Commands.EditProduct
{
    public class EditProductHandler : IRequestHandler<EditProductCommand, Result<Product>>
    {
        private readonly IProductService _productService;
        private readonly IIdentityServies _identityServies;
        private readonly IMapper _mapper;

        public EditProductHandler(IProductService productService,
                                    IIdentityServies identityServies,
                                    IMapper mapper)
        {
            _productService = productService;
            _identityServies = identityServies;
            _mapper = mapper;
        }
        public async Task<Result<Product>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var userId = _identityServies.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Result<Product>.Failure("User must be authenticated to Edit a post.", ErrorType.Unauthorized);

            var oldProduct = await _productService.GetProductByIdAsync(request.Id);

            if (oldProduct == null || oldProduct.Value == null)
                return Result<Product>.Failure("Product not found.", ErrorType.NotFound);

            if (oldProduct.Value.SellerId != userId)
                return Result<Product>.Failure("You can't edit this product. It’s not yours.", ErrorType.Unauthorized);


            var newProduct = _mapper.Map<Product>(request);

            var result = await _productService.EditProductAsync(request.Id , newProduct);

            return result;
        }
    }
}
