using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductFeatures.Commands.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdDto>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;


        public GetProductByIdHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<Result<GetProductByIdDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productResult = await _productService.GetProductByIdAsync(request.Id);

            if (!productResult.IsSuccess)
                return Result<GetProductByIdDto>.Failure(productResult.Message, productResult.ErrorType);

            var dto = _mapper.Map<GetProductByIdDto>(productResult.Value);

            return Result<GetProductByIdDto>.Success(dto);
        }
    }
}
