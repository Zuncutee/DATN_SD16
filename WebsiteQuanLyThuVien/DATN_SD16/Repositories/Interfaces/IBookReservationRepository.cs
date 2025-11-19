using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface cho BookReservation
    /// </summary>
    public interface IBookReservationRepository : IRepository<BookReservation>
    {
        Task<BookReservation?> GetReservationWithDetailsAsync(int reservationId);
        Task<IEnumerable<BookReservation>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<BookReservation>> GetPendingReservationsAsync();
        Task<IEnumerable<BookReservation>> GetReservationsByBookIdAsync(int bookId);
    }
}

