using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng tác giả - sách (BookAuthors) - Quan hệ nhiều-nhiều
    /// </summary>
    [Table("BookAuthors")]
    public class BookAuthor
    {
        [Key]
        public int BookAuthorId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public bool IsPrimary { get; set; } = true; // Tác giả chính hay phụ

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; } = null!;
    }
}

