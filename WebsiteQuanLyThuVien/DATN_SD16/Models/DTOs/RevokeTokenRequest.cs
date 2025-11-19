using System.ComponentModel.DataAnnotations;

namespace DATN_SD16.Models.DTOs
{
    /// <summary>
    /// DTO cho yêu cầu revoke token
    /// </summary>
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

