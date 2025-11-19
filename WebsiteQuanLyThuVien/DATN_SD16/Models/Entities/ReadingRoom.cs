using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng phòng đọc (ReadingRooms)
    /// </summary>
    [Table("ReadingRooms")]
    public class ReadingRoom
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string RoomCode { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; } // Sức chứa

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<ReadingRoomSeat> ReadingRoomSeats { get; set; } = new List<ReadingRoomSeat>();
    }
}

