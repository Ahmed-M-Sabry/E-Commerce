using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Products
{
    public class Photo
    {
        public int Id { get; set; }
        public string ImageName { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId)),JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
