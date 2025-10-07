using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features.CategoryFeatures.Commands.CreateCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.DeleteCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.EditCategory;
using ECommerce.Application.Features.CategoryFeatures.Commands.RestoreCategory;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetAllCategories;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryById;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryByName;
using ECommerce.Application.Features.ProductFeatures.Commands.CreateProduct;
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
    public class AdminAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminAuthorizationBehavior(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var adminRequiredCommands = new[]
            {
                typeof(GetCategoryByNameQuery),
                typeof(GetCategoryByIdQuery),
                typeof(GetAllCategoriesForAdminQuery),
                typeof(RestoreCategoryCommand),
                typeof(EditCategoryCommand),
                typeof(DeleteCategoryCommand),
                typeof(CreateCategoryCommand),


            };

            if (adminRequiredCommands.Contains(request.GetType()))
            {
                if (_httpContextAccessor.HttpContext == null)
                    return CreateUnauthorizedResult<TResponse>("No user context available.");

                var userId = _httpContextAccessor.HttpContext.User?.FindFirst("uid")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return CreateUnauthorizedResult<TResponse>("You Must Be Admin To See It");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return CreateUnauthorizedResult<TResponse>("User not found.");

                if (!await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                    return CreateUnauthorizedResult<TResponse>("You must be an Admin to perform this action.");
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
