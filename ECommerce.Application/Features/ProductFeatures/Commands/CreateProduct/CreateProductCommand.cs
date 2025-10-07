using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<Product>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection PhotosFiles { get; set; }
    }
}