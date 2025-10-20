using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Comman.ProductServiceParam
{
    public class ProductsKeysetPaginationParams
    {
        public string? Cursor { get; set; } = null;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; } = null;
        public int? CategoryId { get; set; } = null;
        public string SortBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = false;
    }


}
