using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng thông báo (Notifications)
    /// </summary>
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; } // Người nhận thông báo

        [Required]
        [MaxLength(50)]
        public string NotificationType { get; set; } = string.Empty; // ReturnReminder, OverdueAlert, BookAvailable, System

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public bool IsEmailSent { get; set; } = false; // Đã gửi email chưa

        public DateTime? EmailSentAt { get; set; }

        public int? RelatedBorrowId { get; set; } // Liên kết với phiếu mượn (nếu có)

        public int? RelatedReservationId { get; set; } // Liên kết với đặt sách (nếu có)

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ReadAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("RelatedBorrowId")]
        public virtual Borrow? RelatedBorrow { get; set; }

        [ForeignKey("RelatedReservationId")]
        public virtual BookReservation? RelatedReservation { get; set; }
    }
}

