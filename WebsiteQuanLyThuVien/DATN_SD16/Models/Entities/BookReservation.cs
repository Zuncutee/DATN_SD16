using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng yêu cầu đặt sách (BookReservations) - Độc giả đặt sách online
    /// </summary>
    [Table("BookReservations")]
    public class BookReservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int UserId { get; set; } // Độc giả đặt

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed, Cancelled

        public DateTime? ExpiryDate { get; set; } // Hết hạn đặt (nếu không lấy sách)

        public int? ApprovedBy { get; set; } // Thủ thư duyệt

        public DateTime? ApprovedAt { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; } // Lý do từ chối

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("ApprovedBy")]
        public virtual User? ApprovedByUser { get; set; }

        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

