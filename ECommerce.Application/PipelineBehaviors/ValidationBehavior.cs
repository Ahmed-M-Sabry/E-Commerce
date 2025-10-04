using FluentValidation;
using MediatR;
using ECommerce.Application.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.PipelineBehaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                var result = await _validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));

                    var responseType = typeof(TResponse);
                    var resultGenericType = responseType.GenericTypeArguments.FirstOrDefault();

                    if (resultGenericType != null)
                    {
                        // Create a Result<T> instance dynamically
                        var failureMethod = typeof(Result<>)
                            .MakeGenericType(resultGenericType)
                            .GetMethod("Failure", new[] { typeof(string), typeof(ErrorType) });

                        if (failureMethod != null)
                        {
                            var failureResult = failureMethod.Invoke(null, new object[] { errors, ErrorType.BadRequest });
                            return (TResponse)failureResult!;
                        }
                    }

                    // fallback
                    throw new Exception("Validation failed but could not construct Result<T>.");
                }
            }

            return await next();
        }

        //public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        //{
        //    if (_validator != null)
        //    {
        //        var result = await _validator.ValidateAsync(request, cancellationToken);

        //        if (!result.IsValid)
        //        {
        //            var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
        //            return Result<TResponse>.Failure(errors, ErrorType.BadRequest) as TResponse;
        //        }
        //    }

        //    return await next();
        //}

    }
}
