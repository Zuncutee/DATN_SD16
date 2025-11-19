using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Service interface cho BookReservation
    /// </summary>
    public interface IBookReservationService
    {
        Task<BookReservation?> GetReservationByIdAsync(int reservationId);
        Task<BookReservation?> GetReservationWithDetailsAsync(int reservationId);
        Task<IEnumerable<BookReservation>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<BookReservation>> GetPendingReservationsAsync();
        Task<BookReservation> CreateReservationAsync(int userId, int bookId);
        Task<bool> ApproveReservationAsync(int reservationId, int approvedBy);
        Task<bool> RejectReservationAsync(int reservationId, int rejectedBy, string reason);
        Task<bool> CancelReservationAsync(int reservationId);
    }
}

