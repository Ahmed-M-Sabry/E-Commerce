using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using ECommerce.Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Queries.GetPhotosByProductId
{

    public class GetPhotosByProductIdHandler : IRequestHandler<GetPhotosByProductIdQuery, Result<IEnumerable<ProductPhoto>>>
    {
        private readonly IProductPhotoService _photoService;

        public GetPhotosByProductIdHandler(IProductPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<Result<IEnumerable<ProductPhoto>>> Handle(GetPhotosByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _photoService.GetPhotosByProductIdAsync(request.ProductId);
        }
    }
}