using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Result<GetProductByIdDto>>
    {
        public int Id { get; set; }

    }
}
