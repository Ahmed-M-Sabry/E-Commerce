using ECommerce.Application.Common;
using ECommerce.Application.IServices.IBasket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Commands.DeleteBasket
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Result<bool>>
    {
        private readonly ICustomerBasketService _basketService;

        public DeleteBasketCommandHandler(ICustomerBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            return await _basketService.DeleteBasketAsync(request.Id);
        }
    }
}
