using ECommerce.Application.Comman;
using ECommerce.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.API.ApplicationBase
{

    public static class ResultExtensions
    {
        public static IActionResult ResultStatusCode<T>(this Result<T> result)
        {
            if (result == null)
            {
                return new ObjectResult(ApiResponse<T>.Fail("Unexpected error occurred.", HttpStatusCode.InternalServerError))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            if (result.IsSuccess)
                return new OkObjectResult(ApiResponse<T>.Success(
                    result.Value,
                    HttpStatusCode.OK,
                    result.Message ?? "Success"
                ));

            var statusCode = result.ErrorType switch
            {
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.BadRequest => HttpStatusCode.BadRequest,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                ErrorType.UnprocessableEntity => HttpStatusCode.UnprocessableEntity,
                ErrorType.InternalServerError => HttpStatusCode.InternalServerError,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError
            };

            return new ObjectResult(ApiResponse<T>.Fail(result.Message, statusCode))
            {
                StatusCode = (int)statusCode
            };
        }

    }
}
