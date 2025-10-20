using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Queries.GetBasketById
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<CustomerBasket>>
    {
        private readonly ICustomerBasketService _basketService;

        public GetBasketQueryHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<CustomerBasket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            return await _basketService.GetBasketAsync(request.Id);
        }
    }
}
