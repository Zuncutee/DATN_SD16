using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng chi tiết kiểm kê (InventoryCheckDetails)
    /// </summary>
    [Table("InventoryCheckDetails")]
    public class InventoryCheckDetail
    {
        [Key]
        public int DetailId { get; set; }

        [Required]
        public int CheckId { get; set; }

        [Required]
        public int CopyId { get; set; }

        [MaxLength(20)]
        public string? ExpectedStatus { get; set; } // Trạng thái trong hệ thống

        [MaxLength(20)]
        public string? ActualStatus { get; set; } // Trạng thái thực tế

        public bool IsMatched { get; set; } = false; // Có khớp không

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("CheckId")]
        public virtual InventoryCheck Check { get; set; } = null!;

        [ForeignKey("CopyId")]
        public virtual BookCopy Copy { get; set; } = null!;
    }
}

