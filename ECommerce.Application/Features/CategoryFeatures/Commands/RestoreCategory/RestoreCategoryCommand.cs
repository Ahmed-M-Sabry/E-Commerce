using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Commands.RestoreCategory
{
    public class RestoreCategoryCommand : IRequest<Result<Category>>
    {
        public int Id { get; set; }
    }
}
