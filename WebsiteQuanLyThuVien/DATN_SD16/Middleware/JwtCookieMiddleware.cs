using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DATN_SD16.Middleware
{
    /// <summary>
    /// Middleware để đọc JWT token từ cookie và set vào HttpContext
    /// </summary>
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtCookieMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Chỉ xử lý nếu chưa có user principal
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                var token = context.Request.Cookies["AuthToken"];

                // Nếu không có trong cookie, thử lấy từ header
                if (string.IsNullOrEmpty(token))
                {
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (authHeader != null && authHeader.StartsWith("Bearer "))
                    {
                        token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var jwtSecretKey = _configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWTTokenGeneration!";
                        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "LibraryManagement";
                        var jwtAudience = _configuration["Jwt:Audience"] ?? "LibraryManagementUsers";

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = System.Text.Encoding.UTF8.GetBytes(jwtSecretKey);

                        var validationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidIssuer = jwtIssuer,
                            ValidateAudience = true,
                            ValidAudience = jwtAudience,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };

                        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                        context.User = principal;
                    }
                    catch
                    {
                        // Token không hợp lệ, xóa cookie
                        context.Response.Cookies.Delete("AuthToken");
                        context.Response.Cookies.Delete("RefreshToken");
                    }
                }
            }

            await _next(context);
        }
    }

    public static class JwtCookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookieAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtCookieMiddleware>();
        }
    }
}

