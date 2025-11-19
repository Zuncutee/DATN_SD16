using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DATN_SD16.Models.Validation
{
    /// <summary>
    /// Helper class cho validation
    /// </summary>
    public static class ValidationHelper
    {
        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult("Email không được để trống");
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                return new ValidationResult("Email không hợp lệ");
            }

            return ValidationResult.Success!;
        }

        public static ValidationResult ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return ValidationResult.Success!; // Phone number is optional
            }

            var phoneRegex = new Regex(@"^[0-9]{10,11}$");
            if (!phoneRegex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Số điện thoại không hợp lệ (10-11 chữ số)");
            }

            return ValidationResult.Success!;
        }

        public static ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult("Mật khẩu không được để trống");
            }

            if (password.Length < 6)
            {
                return new ValidationResult("Mật khẩu phải có ít nhất 6 ký tự");
            }

            return ValidationResult.Success!;
        }

        public static ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new ValidationResult("Tên đăng nhập không được để trống");
            }

            if (username.Length < 3)
            {
                return new ValidationResult("Tên đăng nhập phải có ít nhất 3 ký tự");
            }

            var usernameRegex = new Regex(@"^[a-zA-Z0-9_]+$");
            if (!usernameRegex.IsMatch(username))
            {
                return new ValidationResult("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");
            }

            return ValidationResult.Success!;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }

        public ValidationResult(string? errorMessage = null)
        {
            IsValid = string.IsNullOrEmpty(errorMessage);
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Success => new ValidationResult();
    }
}

