using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeletePhoto
{
    public class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, Result<bool>>
    {
        private readonly IProductPhotoService _photoService;

        public DeletePhotoHandler(IProductPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<Result<bool>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            return await _photoService.DeletePhotoAsync(request.PhotoId);
        }
    }
}
