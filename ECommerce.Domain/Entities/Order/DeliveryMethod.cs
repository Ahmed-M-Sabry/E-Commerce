using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Order
{
    public class DeliveryMethod
    {
        public DeliveryMethod()
        {

        }
        public DeliveryMethod(string name, decimal price, string deliveryTime, string description)
        {
            Name = name;
            Price = price;
            DeliveryTime = deliveryTime;
            Description = description;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
    }
}
