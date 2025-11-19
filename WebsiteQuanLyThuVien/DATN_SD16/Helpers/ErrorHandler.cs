using System.Net;
using System.Text.Json;

namespace DATN_SD16.Helpers
{
    /// <summary>
    /// Helper class để xử lý lỗi
    /// </summary>
    public static class ErrorHandler
    {
        public static IResult HandleError(Exception ex, IWebHostEnvironment env)
        {
            var response = new
            {
                success = false,
                message = env.IsDevelopment() ? ex.Message : "Đã xảy ra lỗi. Vui lòng thử lại sau.",
                detail = env.IsDevelopment() ? ex.StackTrace : null
            };

            return Results.Json(response, statusCode: (int)HttpStatusCode.InternalServerError);
        }

        public static IResult HandleValidationError(string message)
        {
            var response = new
            {
                success = false,
                message = message
            };

            return Results.Json(response, statusCode: (int)HttpStatusCode.BadRequest);
        }

        public static IResult HandleNotFoundError(string resource)
        {
            var response = new
            {
                success = false,
                message = $"Không tìm thấy {resource}"
            };

            return Results.Json(response, statusCode: (int)HttpStatusCode.NotFound);
        }

        public static IResult HandleUnauthorizedError(string message = "Bạn không có quyền thực hiện thao tác này")
        {
            var response = new
            {
                success = false,
                message = message
            };

            return Results.Json(response, statusCode: (int)HttpStatusCode.Unauthorized);
        }
    }
}

