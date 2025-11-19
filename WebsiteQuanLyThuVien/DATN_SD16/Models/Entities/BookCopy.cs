using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng chi tiết bản sách (BookCopies) - Quản lý từng bản sách cụ thể
    /// </summary>
    [Table("BookCopies")]
    public class BookCopy
    {
        [Key]
        public int CopyId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CopyNumber { get; set; } = string.Empty; // Số bản (VD: B001, B002)

        [MaxLength(100)]
        public string? Barcode { get; set; } // Mã vạch

        [MaxLength(20)]
        public string Status { get; set; } = "Available"; // Available, Borrowed, Reserved, Lost, Damaged, Maintenance

        [MaxLength(50)]
        public string Condition { get; set; } = "Good"; // Good, Fair, Poor, Damaged

        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
        public virtual ICollection<BookDamage> BookDamages { get; set; } = new List<BookDamage>();
        public virtual ICollection<InventoryCheckDetail> InventoryCheckDetails { get; set; } = new List<InventoryCheckDetail>();
    }
}

