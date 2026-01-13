using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng cấu hình hệ thống (SystemSettings)
    /// </summary>
    [Table("SystemSettings")]
    public class SystemSetting
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string SettingValue { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; } // Borrowing, Fine, Notification, etc.

        public int? UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UpdatedBy")]
        public virtual User? UpdatedByUser { get; set; }
    }
}

