using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    // Service interface cho Book
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<Book?> GetBookWithDetailsAsync(int bookId);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> SearchBooksAsync(string? title, string? author, int? categoryId, bool? availableOnly);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetMostBorrowedBooksAsync(int top = 10);
        Task<Book> CreateBookAsync(Book book, int createdBy);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int bookId);
        Task<bool> ImportBooksAsync(int bookId, int quantity, int importedBy);
        Task<bool> AddAuthorToBookAsync(int bookId, int authorId, bool isPrimary = true);
        Task<IEnumerable<BookCopy>> GetAvailableCopiesAsync(int bookId);
        Task<bool> UpdateBookAuthorsAsync(int bookId, List<int> authorIds);
    }
}

