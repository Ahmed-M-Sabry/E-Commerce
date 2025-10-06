using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Products
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        public ICollection<Product> products { get; set; } = new HashSet<Product>();
    }
}
