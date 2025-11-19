using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Service interface cho Borrow
    /// </summary>
    public interface IBorrowService
    {
        Task<Borrow?> GetBorrowByIdAsync(int borrowId);
        Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId);
        Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId);
        Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync();
        Task<IEnumerable<Borrow>> GetActiveBorrowsByUserIdAsync(int userId);
        Task<Borrow> CreateBorrowAsync(int userId, int copyId, int borrowedBy, int? reservationId = null);
        Task<bool> ReturnBookAsync(int borrowId, int returnedBy, string? conditionOnReturn = null);
        Task<decimal> CalculateFineAsync(int borrowId);
        Task<bool> RenewBorrowAsync(int borrowId);
    }
}

