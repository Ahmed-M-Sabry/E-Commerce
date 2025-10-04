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

        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(150)]
        public string? StoreName { get; set; }

        [MaxLength(200)]
        public string? StoreAddress { get; set; }

    }
}
