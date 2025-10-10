using ECommerce.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductPhotoFeatures.Commands.DeletePhoto
{
    public class DeletePhotoCommand : IRequest<Result<bool>>
    {
        public int PhotoId { get; set; }
    }
}
