using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng người dùng (Users) - Tất cả người dùng trong hệ thống
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty; // Mật khẩu đã hash

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; } // Male, Female, Other

        public bool IsActive { get; set; } = true; // Trạng thái tài khoản

        public bool IsLocked { get; set; } = false; // Tài khoản bị khóa

        public DateTime? LockedUntil { get; set; } // Thời gian khóa đến khi nào

        public int FailedLoginAttempts { get; set; } = 0; // Số lần đăng nhập sai

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual LibraryCard? LibraryCard { get; set; }
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
        public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        public virtual ICollection<ReadingRoomReservation> ReadingRoomReservations { get; set; } = new List<ReadingRoomReservation>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();
    }
}

