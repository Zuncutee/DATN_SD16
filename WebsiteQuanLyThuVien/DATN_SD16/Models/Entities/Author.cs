using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng tác giả (Authors)
    /// </summary>
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [MaxLength(255)]
        public string AuthorName { get; set; } = string.Empty;

        public string? Biography { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}

