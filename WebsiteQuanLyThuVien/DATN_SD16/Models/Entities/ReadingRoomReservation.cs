using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_SD16.Models.Entities
{
    /// <summary>
    /// Bảng đặt chỗ phòng đọc (ReadingRoomReservations)
    /// </summary>
    [Table("ReadingRoomReservations")]
    public class ReadingRoomReservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public DateTime? CheckInTime { get; set; } // Thời gian check-in bằng QR

        public DateTime? CheckOutTime { get; set; } // Thời gian check-out

        [MaxLength(20)]
        public string Status { get; set; } = "Reserved"; // Reserved, CheckedIn, Completed, Cancelled, NoShow

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("SeatId")]
        public virtual ReadingRoomSeat Seat { get; set; } = null!;
    }
}

