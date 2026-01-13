using System.ComponentModel.DataAnnotations;

namespace DATN_SD16.Models.DTOs
{
    /// <summary>
    /// DTO cho yêu cầu refresh token
    /// </summary>
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

