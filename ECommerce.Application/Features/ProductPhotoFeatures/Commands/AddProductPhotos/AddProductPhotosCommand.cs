using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.AddProductPhotos
{
    public class AddProductPhotosCommand : IRequest<Result<List<ProductPhoto>>>
    {
        public int ProductId { get; set; }
        public IFormFileCollection PhotosFiles { get; set; }
    }
}
