using ECommerce.Application.Common;
using ECommerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using ECommerce.Application.IServices;
using ECommerce.Application.IServices.IProductServ;
using MediatR;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    private readonly IProductService _productService;
    private readonly IIdentityServies _identityServies;

    public DeleteProductHandler(IProductService productService, IIdentityServies identityServies)
    {
        _productService = productService;
        _identityServies = identityServies;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityServies.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result<bool>.Failure("User must be authenticated.", ErrorType.Unauthorized);

        var productResult = await _productService.GetProductByIdAsync(request.Id);
        if (!productResult.IsSuccess || productResult.Value == null)
            return Result<bool>.Failure("Product not found.", ErrorType.NotFound);

        var product = productResult.Value;

        if (product.SellerId != userId)
            return Result<bool>.Failure("You are not authorized to delete this product.", ErrorType.Forbidden);

        return await _productService.DeleteProductAsync(request.Id);
    }
}
