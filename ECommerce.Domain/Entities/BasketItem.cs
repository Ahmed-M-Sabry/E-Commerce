using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class BasketItem
    {
        public int Id { get; set; } 
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int quantity { get; set; } = 1;
        public decimal price { get; set; }
        public string category { get; set; }
    }
}
