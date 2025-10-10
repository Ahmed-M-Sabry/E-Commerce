using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public virtual List<string> PhotosName { get; set; }
        public string CategoryName { get; set; }
        public double Rating { get; set; }
        public string SellerName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
