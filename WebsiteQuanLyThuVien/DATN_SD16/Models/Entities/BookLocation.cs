using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng vị trí sách trong thư viện (BookLocations)
    /// </summary>
    [Table("BookLocations")]
    public class BookLocation
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationCode { get; set; } = string.Empty; // VD: A-01-001

        [MaxLength(50)]
        public string? ShelfNumber { get; set; } // Số kệ

        [MaxLength(50)]
        public string? RowNumber { get; set; } // Số hàng

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

