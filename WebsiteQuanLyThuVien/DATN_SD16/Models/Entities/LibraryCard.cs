using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng thẻ thư viện (LibraryCards) - Đăng ký thẻ cho độc giả
    /// </summary>
    [Table("LibraryCards")]
    public class LibraryCard
    {
        [Key]
        public int CardId { get; set; }

        [Required]
        public int UserId { get; set; } // Mỗi độc giả có 1 thẻ

        [Required]
        [MaxLength(50)]
        public string CardNumber { get; set; } = string.Empty; // Số thẻ thư viện

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Now; // Ngày cấp thẻ

        public DateTime? ExpiryDate { get; set; } // Ngày hết hạn thẻ

        [MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Expired, Suspended, Cancelled

        public int? CreatedBy { get; set; } // Thủ thư tạo thẻ

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }
    }
}

