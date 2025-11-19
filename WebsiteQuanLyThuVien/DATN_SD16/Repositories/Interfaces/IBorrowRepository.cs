using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface cho Borrow
    /// </summary>
    public interface IBorrowRepository : IRepository<Borrow>
    {
        Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId);
        Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId);
        Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync();
        Task<IEnumerable<Borrow>> GetActiveBorrowsByUserIdAsync(int userId);
        Task<decimal> CalculateFineAsync(int borrowId);
    }
}

