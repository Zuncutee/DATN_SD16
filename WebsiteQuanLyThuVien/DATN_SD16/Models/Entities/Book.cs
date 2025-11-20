using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng sách (Books) - Thông tin chung về sách
    /// </summary>
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [MaxLength(50)]
        public string? ISBN { get; set; } // Mã ISBN

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } // Mô tả sách

        [MaxLength(500)]
        public string? CoverImage { get; set; } // Đường dẫn ảnh bìa

        [MaxLength(50)]
        public string Language { get; set; } = "Tiếng Việt";

        public int? PublicationYear { get; set; }

        public int? PageCount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? PublisherId { get; set; }

        public int? LocationId { get; set; } // Vị trí sách trong thư viện

        public int TotalCopies { get; set; } = 0; // Tổng số bản

        public int AvailableCopies { get; set; } = 0; // Số bản có sẵn

        public int BorrowedCopies { get; set; } = 0; // Số bản đang mượn

        public int LostCopies { get; set; } = 0; // Số bản mất

        public int DamagedCopies { get; set; } = 0; // Số bản hỏng

        [MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Archived

        public int? CreatedBy { get; set; } // Người tạo (Admin)

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public virtual Category? Category { get; set; }

        [ForeignKey("PublisherId")]
        [ValidateNever]
        public virtual Publisher? Publisher { get; set; }

        [ForeignKey("LocationId")]
        [ValidateNever]
        public virtual BookLocation? Location { get; set; }

        [ForeignKey("CreatedBy")]
        [ValidateNever]
        public virtual User? CreatedByUser { get; set; }

        [ValidateNever]
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        [ValidateNever]
        public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
        [ValidateNever]
        public virtual ICollection<BookImport> BookImports { get; set; } = new List<BookImport>();
        [ValidateNever]
        public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();
        [ValidateNever]
        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
    }
}

