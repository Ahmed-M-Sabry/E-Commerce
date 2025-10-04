using ECommerce.Domain.AuthenticationHepler;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Buyer and Seller
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<RefreshToken>? refreshTokens { get; set; }


        // Seller Only

        public string? Address { get; set; }

        public string? StoreName { get; set; }

        public string? StoreAddress { get; set; }

    }
}
