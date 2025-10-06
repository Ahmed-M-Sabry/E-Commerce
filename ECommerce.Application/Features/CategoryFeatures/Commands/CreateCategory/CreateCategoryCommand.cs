using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Result<Category>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
