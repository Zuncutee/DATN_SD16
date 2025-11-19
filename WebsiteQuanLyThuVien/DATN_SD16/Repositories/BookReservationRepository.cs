using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Repositories
{
    /// <summary>
    /// Repository implementation cho BookReservation
    /// </summary>
    public class BookReservationRepository : Repository<BookReservation>, IBookReservationRepository
    {
        public BookReservationRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<BookReservation?> GetReservationWithDetailsAsync(int reservationId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.Book)
                    .ThenInclude(b => b.Category)
                .Include(r => r.ApprovedByUser)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        public async Task<IEnumerable<BookReservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(r => r.Book)
                    .ThenInclude(b => b.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookReservation>> GetPendingReservationsAsync()
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.Book)
                    .ThenInclude(b => b.Category)
                .Where(r => r.Status == "Pending")
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookReservation>> GetReservationsByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }
    }
}

