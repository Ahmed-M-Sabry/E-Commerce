using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesForAdminQuery : IRequest<Result<IEnumerable<Category>>>
    {

    }
}
