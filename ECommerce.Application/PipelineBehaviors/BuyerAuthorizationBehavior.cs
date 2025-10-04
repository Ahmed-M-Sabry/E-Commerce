using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using ECommerce.Application.Features;
using ECommerce.Domain.AuthenticationHepler;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.PipelineBehaviors
{
    internal class BuyerAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuyerAuthorizationBehavior(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // List of commands that require Admin role
            var userRequiredCommands = new[]
            {
                // Create
                                typeof(ses)


                // Edit
                

                // Delete
                

                // Restore
                
                
                // Get All


                

            };

            // Check if the request is one of the admin-required commands
            if (userRequiredCommands.Contains(request.GetType()))
            {
                // If there's an HTTP context (i.e., request comes from Controller)
                if (_httpContextAccessor.HttpContext != null)
                {
                    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("uid")?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Result<TResponse>.Failure("User ID not found in token.", ErrorType.Unauthorized) as TResponse;

                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                        return Result<TResponse>.Failure("User not found.", ErrorType.Unauthorized) as TResponse;

                    if (!await _userManager.IsInRoleAsync(user, ApplicationRoles.Buyer))
                        return Result<TResponse>.Failure("You must be an user to perform this action.", ErrorType.Unauthorized) as TResponse;
                }
                else
                {
                    // If no HTTP context (e.g., called from internal service), fail by default
                    return Result<TResponse>.Failure("No user context available.", ErrorType.Unauthorized) as TResponse;
                }
            }

            // Proceed to the next handler
            return await next();
        }
    }
}
