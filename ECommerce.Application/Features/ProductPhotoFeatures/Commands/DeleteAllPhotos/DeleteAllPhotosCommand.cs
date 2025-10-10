using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeleteAllPhotos
{
    public class DeleteAllPhotosCommand : IRequest<Result<bool>>
    {
        public int ProductId { get; set; }
    }
}
