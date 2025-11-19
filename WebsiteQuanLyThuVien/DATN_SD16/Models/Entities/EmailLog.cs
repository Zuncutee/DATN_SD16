using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng lịch sử gửi email (EmailLogs)
    /// </summary>
    [Table("EmailLogs")]
    public class EmailLog
    {
        [Key]
        public int EmailLogId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string EmailTo { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string EmailSubject { get; set; } = string.Empty;

        [Required]
        public string EmailBody { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string EmailType { get; set; } = string.Empty; // ReturnReminder, OverdueAlert, BookAvailable, PasswordReset

        public bool IsSent { get; set; } = false;

        public DateTime? SentAt { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}

