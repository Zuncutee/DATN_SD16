using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng chỗ ngồi (ReadingRoomSeats)
    /// </summary>
    [Table("ReadingRoomSeats")]
    public class ReadingRoomSeat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SeatNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? QRCode { get; set; } // Mã QR để check-in

        [MaxLength(20)]
        public string Status { get; set; } = "Available"; // Available, Reserved, Occupied, Maintenance

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("RoomId")]
        public virtual ReadingRoom Room { get; set; } = null!;

        public virtual ICollection<ReadingRoomReservation> ReadingRoomReservations { get; set; } = new List<ReadingRoomReservation>();
    }
}

