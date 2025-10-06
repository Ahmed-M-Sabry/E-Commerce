using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Result<Category>>
    {
        public int Id { get; set; }
    }
}
