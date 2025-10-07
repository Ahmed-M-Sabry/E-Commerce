using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features;
using ECommerce.Application.Features.CategoryFeatures.Queries.GetCategoryByName;
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
    public class PublicAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public PublicAuthorizationBehavior(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var publicRequiredCommands = new[]
            {
                typeof(ses),
            };

            if (publicRequiredCommands.Contains(typeof(TRequest)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return CreateUnauthorizedResult<TResponse>("No HttpContext found.");

                var user = await _userManager.GetUserAsync(httpContext.User);
                if (user == null)
                    return CreateUnauthorizedResult<TResponse>("User not found.");
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
