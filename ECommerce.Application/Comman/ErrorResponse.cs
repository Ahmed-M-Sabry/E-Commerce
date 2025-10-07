using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Comman
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StackTrace { get; set; }

        public ErrorResponse(int statusCodel)
        {
            StatusCode = statusCodel;
        }

        public ErrorResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        public ErrorResponse(int statusCode, string? message = null, string? stackTrace = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            StackTrace = stackTrace;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Resource not found",
                500 => "Internal Server Error",
                _ => "An unexpected error occurred"
            };
        }


    }
}
