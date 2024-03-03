using Microsoft.AspNetCore.Mvc;

namespace prod_server.Classes
{
    public class BaseController: ControllerBase
    {
        protected IActionResult ApiResponse<T>(int statusCode, string message, T? payload = default)
        {
            var response = new IResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Payload = payload
            };

            return new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        protected IActionResult Ok<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status200OK, message, payload);
        }
        protected IActionResult Ok(string message)
        {
            return ApiResponse<object>(StatusCodes.Status200OK, message, null);
        }

        protected IActionResult BadRequest<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status400BadRequest, message, payload);
        }
        protected IActionResult BadRequest(string message = "bad request")
        {
            return ApiResponse<object>(StatusCodes.Status400BadRequest, message, null);
        }

        protected IActionResult Unauthorized<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status401Unauthorized, message, payload);
        }
        protected IActionResult Unauthorized(string message = "unauthorized")
        {
            return ApiResponse<object>(StatusCodes.Status401Unauthorized, message, null);
        }

        protected IActionResult NotFound<T>(string message, T payload = default)
        {
            return ApiResponse<T>(StatusCodes.Status401Unauthorized, message, payload);
        }
        protected IActionResult NotFound(string message = "not found")
        {
            return ApiResponse<object>(StatusCodes.Status404NotFound, message, null);
        }


    }
}
