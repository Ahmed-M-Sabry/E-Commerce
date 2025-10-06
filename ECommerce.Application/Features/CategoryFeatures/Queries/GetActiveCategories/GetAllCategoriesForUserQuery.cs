using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CategoryFeatures.Queries.GetActiveCategories
{
    [AllowAnonymous]
    public class GetAllCategoriesForUserQuery : IRequest<Result<IEnumerable<Category>>>
    {
    }
}
