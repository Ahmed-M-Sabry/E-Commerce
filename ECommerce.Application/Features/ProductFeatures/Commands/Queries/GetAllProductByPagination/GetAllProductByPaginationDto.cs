using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetAllProductByPagination
{
    public class GetAllProductByPaginationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public virtual List<string> PhotosName { get; set; }
        public string CategoryName { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
