using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface cho Book
    /// </summary>
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetBookWithDetailsAsync(int bookId);
        Task<IEnumerable<Book>> SearchBooksAsync(string? title, string? author, int? categoryId, bool? availableOnly);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetMostBorrowedBooksAsync(int top = 10);
    }
}

