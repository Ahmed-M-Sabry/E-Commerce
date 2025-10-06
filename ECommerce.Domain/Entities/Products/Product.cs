using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId)) , JsonIgnore]
        public virtual Category Category { get; set; }

        public string SellerId { get; set; }
        [ForeignKey(nameof(SellerId)) , JsonIgnore]
        public ApplicationUser Seller { get; set; }

        public double rating { get; set; }
    }
}
