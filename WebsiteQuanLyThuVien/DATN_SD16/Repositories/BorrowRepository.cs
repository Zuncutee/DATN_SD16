using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Repositories
{
    /// <summary>
    /// Repository implementation cho Borrow
    /// </summary>
    public class BorrowRepository : Repository<Borrow>, IBorrowRepository
    {
        public BorrowRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId)
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Reservation)
                .Include(b => b.BorrowedByUser)
                .Include(b => b.ReturnedByUser)
                .FirstOrDefaultAsync(b => b.BorrowId == borrowId);
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Where(b => b.Status == "Borrowed" 
                    && b.DueDate < DateTime.Now 
                    && b.ReturnDate == null)
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetActiveBorrowsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Where(b => b.UserId == userId && b.Status == "Borrowed")
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<decimal> CalculateFineAsync(int borrowId)
        {
            var borrow = await GetBorrowWithDetailsAsync(borrowId);
            if (borrow == null || borrow.ReturnDate == null)
                return 0;

            if (borrow.ReturnDate <= borrow.DueDate)
                return 0;

            var daysOverdue = (borrow.ReturnDate.Value - borrow.DueDate).Days;
            var finePerDay = await _context.SystemSettings
                .Where(s => s.SettingKey == "FinePerDay")
                .Select(s => decimal.Parse(s.SettingValue))
                .FirstOrDefaultAsync();

            return daysOverdue * finePerDay;
        }
    }
}

