using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.BasketFeatures.Queries
{
    public class GetBasketQuery : IRequest<Result<CustomerBasket>>
    {
        public string Id { get; set; }
    }
}
