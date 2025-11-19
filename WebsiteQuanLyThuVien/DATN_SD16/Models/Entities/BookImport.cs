using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng lịch sử nhập sách (BookImports) - Ghi lại khi nhập sách mới
    /// </summary>
    [Table("BookImports")]
    public class BookImport
    {
        [Key]
        public int ImportId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; } // Số lượng nhập

        [Required]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        [Required]
        public int ImportedBy { get; set; } // Người nhập (Admin/Thủ thư)

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("ImportedBy")]
        public virtual User ImportedByUser { get; set; } = null!;
    }
}

