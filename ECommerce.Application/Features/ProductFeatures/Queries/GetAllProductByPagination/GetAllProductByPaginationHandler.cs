using AutoMapper;
using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.IServices.IProductServ;
using MediatR;

namespace ECommerce.Application.Features.ProductFeatures.Queries.GetAllProductByPagination
{
    public class GetAllProductByPaginationHandler
        : IRequestHandler<GetAllProductByPaginationQuery, Result<PagedResult<GetAllProductByPaginationDto>>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllProductByPaginationHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllProductByPaginationDto>>> Handle(
            GetAllProductByPaginationQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllProductsPaginationAsync(new ProductParams
            {
                Search = request.Search,
                CategoryId = request.CategoryId,
                SortOption = request.SortOption,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });

            if (!result.IsSuccess)
                return Result<PagedResult<GetAllProductByPaginationDto>>.Failure(result.Message, result.ErrorType);

            var mappedData = _mapper.Map<List<GetAllProductByPaginationDto>>(result.Value.Data);

            var paged = new PagedResult<GetAllProductByPaginationDto>
            {
                Data = mappedData,
                TotalCount = result.Value.TotalCount,
                PageNumber = result.Value.PageNumber,
                PageSize = result.Value.PageSize
            };

            return Result<PagedResult<GetAllProductByPaginationDto>>.Success(paged);
        }
    }
}
