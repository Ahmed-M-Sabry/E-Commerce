using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.AddProductPhotos
{
    public class AddProductPhotosHandler : IRequestHandler<AddProductPhotosCommand, Result<List<ProductPhoto>>>
    {
        private readonly IProductPhotoService _photoService;

        public AddProductPhotosHandler(IProductPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<Result<List<ProductPhoto>>> Handle(AddProductPhotosCommand request, CancellationToken cancellationToken)
        {
            var result = await _photoService.AddPhotosAsync(request.ProductId, request.PhotosFiles);
            return result;
        }
    }
}
