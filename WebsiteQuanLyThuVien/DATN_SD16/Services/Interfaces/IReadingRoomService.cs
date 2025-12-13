using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    public interface IReadingRoomService
    {
        // Room Management
        Task<ReadingRoom?> GetRoomByIdAsync(int roomId);
        Task<IEnumerable<ReadingRoom>> GetAllRoomsAsync();
        Task<IEnumerable<ReadingRoom>> GetActiveRoomsAsync();
        Task<ReadingRoom?> GetRoomWithSeatsAsync(int roomId);
        Task<ReadingRoom> CreateRoomAsync(ReadingRoom room, int createdBy);
        Task<bool> UpdateRoomAsync(ReadingRoom room);
        Task<bool> DeleteRoomAsync(int roomId);

        // Seat Management
        Task<ReadingRoomSeat?> GetSeatByIdAsync(int seatId);
        Task<IEnumerable<ReadingRoomSeat>> GetSeatsByRoomIdAsync(int roomId);
        Task<IEnumerable<ReadingRoomSeat>> GetAvailableSeatsAsync(int roomId);
        Task<ReadingRoomSeat> CreateSeatAsync(ReadingRoomSeat seat);
        Task<bool> UpdateSeatAsync(ReadingRoomSeat seat);
        Task<bool> DeleteSeatAsync(int seatId);
        Task<string> GenerateQRCodeForSeatAsync(int seatId);

        // Reservation Management
        Task<ReadingRoomReservation?> GetReservationByIdAsync(int reservationId);
        Task<IEnumerable<ReadingRoomReservation>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<ReadingRoomReservation>> GetActiveReservationsAsync();
        Task<ReadingRoomReservation> CreateReservationAsync(int userId, int seatId, DateTime reservationDate, TimeSpan startTime, TimeSpan endTime);
        Task<bool> CheckInAsync(int reservationId, string qrCode);
        Task<bool> CheckOutAsync(int reservationId);
        Task<bool> CancelReservationAsync(int reservationId);
    }
}
