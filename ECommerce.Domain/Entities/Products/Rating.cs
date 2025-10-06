using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Products
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int Stars { get; set; }
        public string content { get; set; }
        public DateTime Review { get; set; } = DateTime.Now;

        public string BuyerId { get; set; }
        [ForeignKey(nameof(BuyerId)) , JsonIgnore]
        public virtual ApplicationUser Buyer { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId)),JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
