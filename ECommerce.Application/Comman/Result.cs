using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Message { get; } 
        public ErrorType ErrorType { get; }

        private Result(T value, string message = null)
        {
            IsSuccess = true;
            Value = value;
            Message = message;
            ErrorType = ErrorType.None;
        }

        private Result(string message, ErrorType errorType)
        {
            IsSuccess = false;
            Value = default;
            Message = message;
            ErrorType = errorType;
        }

        public static Result<T> Success(T value, string message = null) => new Result<T>(value, message);
        public static Result<T> Failure(string message, ErrorType errorType) => new Result<T>(message, errorType);
    }

    public enum ErrorType
    {
        None,
        NotFound,
        BadRequest,
        Conflict,
        UnprocessableEntity,
        InternalServerError,
        Unauthorized,
        Forbidden
    }
}