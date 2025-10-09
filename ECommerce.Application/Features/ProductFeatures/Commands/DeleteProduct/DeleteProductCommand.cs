using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

    }
}
