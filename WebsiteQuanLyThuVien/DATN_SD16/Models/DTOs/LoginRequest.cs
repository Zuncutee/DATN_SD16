using System.ComponentModel.DataAnnotations;

namespace DATN_SD16.Models.DTOs
{
    /// <summary>
    /// DTO cho yêu cầu đăng nhập
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = string.Empty;
    }
}

