using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng phiếu mượn (Borrows) - Phiếu mượn sách
    /// </summary>
    [Table("Borrows")]
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BorrowNumber { get; set; } = string.Empty; // Số phiếu mượn

        [Required]
        public int UserId { get; set; } // Độc giả mượn

        [Required]
        public int CopyId { get; set; } // Bản sách cụ thể

        public int? ReservationId { get; set; } // Liên kết với đặt sách (nếu có)

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; } // Ngày hẹn trả

        public DateTime? ReturnDate { get; set; } // Ngày trả thực tế

        [MaxLength(20)]
        public string Status { get; set; } = "Borrowed"; // Borrowed, Returned, Overdue, Lost, Damaged

        [Column(TypeName = "decimal(18,2)")]
        public decimal FineAmount { get; set; } = 0; // Tiền phạt

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinePaid { get; set; } = 0; // Tiền phạt đã trả

        [MaxLength(50)]
        public string? ConditionOnBorrow { get; set; } // Tình trạng sách khi mượn

        [MaxLength(50)]
        public string? ConditionOnReturn { get; set; } // Tình trạng sách khi trả

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int BorrowedBy { get; set; } // Thủ thư cho mượn

        public int? ReturnedBy { get; set; } // Thủ thư nhận trả

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CopyId")]
        public virtual BookCopy Copy { get; set; } = null!;

        [ForeignKey("ReservationId")]
        public virtual BookReservation? Reservation { get; set; }

        [ForeignKey("BorrowedBy")]
        public virtual User BorrowedByUser { get; set; } = null!;

        [ForeignKey("ReturnedBy")]
        public virtual User? ReturnedByUser { get; set; }

        public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

