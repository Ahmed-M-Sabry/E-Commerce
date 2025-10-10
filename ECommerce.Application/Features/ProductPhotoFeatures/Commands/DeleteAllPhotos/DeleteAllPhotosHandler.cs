using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeleteAllPhotos
{
    public class DeleteAllPhotosHandler : IRequestHandler<DeleteAllPhotosCommand, Result<bool>>
    {
        private readonly IProductPhotoService _photoService;

        public DeleteAllPhotosHandler(IProductPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<Result<bool>> Handle(DeleteAllPhotosCommand request, CancellationToken cancellationToken)
        {
            return await _photoService.DeleteAllPhotosAsync(request.ProductId);
        }
    }
}
