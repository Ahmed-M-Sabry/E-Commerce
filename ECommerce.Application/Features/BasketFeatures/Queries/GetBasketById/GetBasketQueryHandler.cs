//using ECommerce.Application.Common;
//using ECommerce.Application.IServices.IBasket;
//using ECommerce.Domain.Entities;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ECommerce.Application.Features.BasketFeatures.Queries.GetBasketById
//{
//    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<CustomerBasket>>
//    {
//        private readonly ICustomerBasketService _basketService;

//        public GetBasketQueryHandler(ICustomerBasketService basketService)
//        {
//            _basketService = basketService;
//        }

//        public async Task<Result<CustomerBasket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
//        {
//            return await _basketService.GetBasketAsync(request.Id);
//        }
//    }
//}

using ECommerce.Application.Common;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Queries.GetBasketById
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<CustomerBasket>>
    {
        private readonly ICustomerBasketService _basketService;
        private readonly IProductService _productService;

        public GetBasketQueryHandler(ICustomerBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<Result<CustomerBasket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Result<CustomerBasket>.Failure("Basket ID is required.", ErrorType.BadRequest);

            var basketResult = await _basketService.GetBasketAsync(request.Id);
            if (!basketResult.IsSuccess)
                return basketResult;

            var basket = basketResult.Value;

            foreach (var item in basket.basketItems.ToList()) 
            {
                var productResult = await _productService.GetProductByIdAsync(item.Id);
                if (productResult.IsSuccess)
                {
                    var product = productResult.Value;
                    item.name = product.Name;
                    item.price = product.NewPrice;
                    item.description = product.Description;
                    item.category = product.Category?.Name ?? "Unknown";
                    item.image = product.Photos?.FirstOrDefault()?.ImageName ?? "/Uploads/Defaults/defaultphoto.png";

                    if (item.quantity > product.StockQuantity)
                    {
                        if (product.StockQuantity > 0)
                        {
                            item.quantity = product.StockQuantity; 
                        }
                        else
                        {
                            basket.basketItems.Remove(item); 
                        }
                    }
                }
                else
                {
                    basket.basketItems.Remove(item);
                }
            }

            var updated = await _basketService.UpdateBasketAsync(basket);
            if (!updated.IsSuccess)
                return Result<CustomerBasket>.Failure("Failed to update basket.", ErrorType.InternalServerError);

            return Result<CustomerBasket>.Success(updated.Value);
        }
    }
}