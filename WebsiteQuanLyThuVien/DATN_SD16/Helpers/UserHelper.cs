using System.Security.Claims;

namespace DATN_SD16.Helpers
{
    /// <summary>
    /// Helper class để lấy thông tin user từ Claims
    /// </summary>
    public static class UserHelper
    {
        public static int? GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        public static string? GetUsername(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string? GetEmail(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetFullName(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        public static List<string> GetRoles(ClaimsPrincipal user)
        {
            return user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        public static bool IsInRole(ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }

        public static bool HasAnyRole(ClaimsPrincipal user, params string[] roles)
        {
            return roles.Any(role => user.IsInRole(role));
        }
    }
}

