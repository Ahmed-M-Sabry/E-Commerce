using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.EditBasket
{
    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Result<CustomerBasket>>
    {
        private readonly ICustomerBasketService _basketService;

        public CreateBasketCommandHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<CustomerBasket>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = new CustomerBasket
            {
                Id = request.Id,
                basketItems = request.Items
            };

            return await _basketService.UpdateBasketAsync(basket);
        }
    }
}
