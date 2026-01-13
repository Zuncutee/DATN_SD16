using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng phiếu kiểm kê (InventoryChecks)
    /// </summary>
    [Table("InventoryChecks")]
    public class InventoryCheck
    {
        [Key]
        public int CheckId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CheckNumber { get; set; } = string.Empty;

        [Required]
        public DateTime CheckDate { get; set; } = DateTime.Now;

        [Required]
        public int CheckedBy { get; set; } // Người kiểm kê (Thủ thư)

        [MaxLength(20)]
        public string Status { get; set; } = "InProgress"; // InProgress, Completed, Cancelled

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CheckedBy")]
        public virtual User CheckedByUser { get; set; } = null!;

        public virtual ICollection<InventoryCheckDetail> InventoryCheckDetails { get; set; } = new List<InventoryCheckDetail>();
    }
}

