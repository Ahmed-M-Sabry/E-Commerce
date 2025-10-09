using ECommerce.Application.Comman;
using ECommerce.Application.Comman.Enum;
using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetAllProductByPagination
{
    public class GetAllProductByPaginationQuery : IRequest<Result<PagedResult<GetAllProductByPaginationDto>>>
    {

        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public ProductSortOption SortOption { get; set; } = ProductSortOption.NameAsc;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


}
