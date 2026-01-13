using DATN_SD16.Models.Entities;
using System.Security.Claims;

namespace DATN_SD16.Services.Interfaces
{
    // Service interface cho JWT
    public interface IJwtService
    {
        string GenerateToken(User user, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
    }
}

