using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng lịch sử mượn (BorrowHistory) - Lưu lịch sử đầy đủ
    /// </summary>
    [Table("BorrowHistory")]
    public class BorrowHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int BorrowId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CopyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty; // Borrow, Return, Renew, Overdue

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("BorrowId")]
        public virtual Borrow Borrow { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CopyId")]
        public virtual BookCopy Copy { get; set; } = null!;
    }
}

