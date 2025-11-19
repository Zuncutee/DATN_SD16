using DATN_SD16.Models.DTOs;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Service interface cho Authentication
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}

