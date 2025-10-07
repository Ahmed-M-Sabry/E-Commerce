using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryByName;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
using ECommerce.Application.Features.ProductFeatures.Commands.EditProduct;
using ECommerce.Domain.AuthenticationHepler;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.PipelineBehaviors
{
    public class SellerAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerAuthorizationBehavior(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var sellerRequiredCommands = new[]
            {
                typeof(CreateProductCommand),
                typeof(EditProductCommand   ),
            };

            if (sellerRequiredCommands.Contains(typeof(TRequest)))
            {

                var userId = _httpContextAccessor.HttpContext.User?.FindFirst("uid")?.Value; 
                if (string.IsNullOrEmpty(userId)) 
                    return CreateUnauthorizedResult<TResponse>("You Must Login");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return CreateUnauthorizedResult<TResponse>("User not found.");

                if (await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                    return await next();

                if (!await _userManager.IsInRoleAsync(user, ApplicationRoles.Seller))
                    return CreateUnauthorizedResult<TResponse>("You must be a Seller to perform this action.");
            }

            return await next();
        }

        private static TResponse CreateUnauthorizedResult<TResponse>(string message)
        {
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var innerType = responseType.GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(innerType)
                    .GetMethod("Failure", new[] { typeof(string), typeof(ErrorType) });

                if (failureMethod != null)
                {
                    var failure = failureMethod.Invoke(null, new object[] { message, ErrorType.Unauthorized });
                    return (TResponse)failure!;
                }
            }

            throw new InvalidOperationException($"Unauthorized access for {typeof(TRequest).Name}, but could not construct Result<T> response.");
        }
    }
}
