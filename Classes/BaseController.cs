using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes.Others;
using prod_server.Entities;

namespace prod_server.Classes
{

    public class BaseController : ControllerBase
    {
        public class IResponse<T>
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public T? Payload { get; set; }

        }

        protected IResponse<T> ApiResponse<T>(int statusCode, string message, T? payload = default)
        {
            return new IResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Payload = payload
            };
        }

        protected IResponse<T> Ok<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status200OK, message, payload);
        }

        protected IResponse<object> Ok(string message)
        {
            return ApiResponse<object>(StatusCodes.Status200OK, message, null);
        }

        protected IResponse<T> BadRequest<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status400BadRequest, message, payload);
        }

        protected IResponse<object> BadRequest(string message = "bad request")
        {
            return ApiResponse<object>(StatusCodes.Status400BadRequest, message, null);
        }

        protected IResponse<T> Unauthorized<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status401Unauthorized, message, payload);
        }

        protected IResponse<object> Unauthorized(string message = "unauthorized")
        {
            return ApiResponse<object>(StatusCodes.Status401Unauthorized, message, null);
        }

        protected IResponse<T> NotFound<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status404NotFound, message, payload);
        }

        protected IResponse<object> NotFound(string message = "not found")
        {
            return ApiResponse<object>(StatusCodes.Status404NotFound, message, null);
        }


        protected IResponse<T> UnexpectedError<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status500InternalServerError, message, payload);
        }

        protected IResponse<object> UnexpectedError(string message = "Server Error")
        {
            return ApiResponse<object>(StatusCodes.Status500InternalServerError, message, null);
        }
    }
}
