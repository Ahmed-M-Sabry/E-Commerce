using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Order
{
    public class ShippingAddress
    {
        public ShippingAddress()
        {

        }
        public ShippingAddress(string firstName, string lastName, string city, string zipCode, string street, string state)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            ZipCode = zipCode;
            Street = street;
            State = state;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
    }
}
