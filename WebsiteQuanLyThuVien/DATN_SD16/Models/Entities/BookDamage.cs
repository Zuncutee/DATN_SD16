using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng báo cáo sách hỏng/mất (BookDamages)
    /// </summary>
    [Table("BookDamages")]
    public class BookDamage
    {
        [Key]
        public int DamageId { get; set; }

        [Required]
        public int CopyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DamageType { get; set; } = string.Empty; // Lost, Damaged, Missing

        [Required]
        public DateTime DamageDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int ReportedBy { get; set; } // Người báo cáo (Thủ thư)

        [MaxLength(20)]
        public string Status { get; set; } = "Reported"; // Reported, Processed, Resolved

        public int? ProcessedBy { get; set; } // Người xử lý (Admin)

        public DateTime? ProcessedAt { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CopyId")]
        public virtual BookCopy Copy { get; set; } = null!;

        [ForeignKey("ReportedBy")]
        public virtual User ReportedByUser { get; set; } = null!;

        [ForeignKey("ProcessedBy")]
        public virtual User? ProcessedByUser { get; set; }
    }
}

