using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Book
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<BookImport> _bookImportRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly LibraryDbContext _context;

        public BookService(
            IBookRepository bookRepository,
            IRepository<BookImport> bookImportRepository,
            IRepository<BookAuthor> bookAuthorRepository,
            IRepository<BookCopy> bookCopyRepository,
            LibraryDbContext context)
        {
            _bookRepository = bookRepository;
            _bookImportRepository = bookImportRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookCopyRepository = bookCopyRepository;
            _context = context;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(bookId);
        }

        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            return await _bookRepository.GetBookWithDetailsAsync(bookId);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string? title, string? author, int? categoryId, bool? availableOnly)
        {
            return await _bookRepository.SearchBooksAsync(title, author, categoryId, availableOnly);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetMostBorrowedBooksAsync(int top = 10)
        {
            return await _bookRepository.GetMostBorrowedBooksAsync(top);
        }

        public async Task<Book> CreateBookAsync(Book book, int createdBy)
        {
            book.CreatedBy = createdBy;
            book.CreatedAt = DateTime.Now;
            book.UpdatedAt = DateTime.Now;
            book.Status = "Active";
            return await _bookRepository.AddAsync(book);
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            var existing = await _bookRepository.GetByIdAsync(book.BookId);
            if (existing == null) return false;

            existing.Title = book.Title;
            existing.ISBN = book.ISBN;
            existing.Description = book.Description;
            existing.CoverImage = string.IsNullOrWhiteSpace(book.CoverImage) ? existing.CoverImage : book.CoverImage;
            existing.Language = book.Language;
            existing.PublicationYear = book.PublicationYear;
            existing.PageCount = book.PageCount;
            existing.CategoryId = book.CategoryId;
            existing.PublisherId = book.PublisherId;
            existing.LocationId = book.LocationId;
            existing.TotalCopies = book.TotalCopies;
            existing.AvailableCopies = book.AvailableCopies;
            existing.BorrowedCopies = book.BorrowedCopies;
            existing.LostCopies = book.LostCopies;
            existing.DamagedCopies = book.DamagedCopies;
            existing.Status = book.Status;
            existing.UpdatedAt = DateTime.Now;

            await _bookRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null) return false;

            book.Status = "Archived";
            await _bookRepository.UpdateAsync(book);
            return true;
        }

        public async Task<bool> ImportBooksAsync(int bookId, int quantity, int importedBy)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null) return false;

            // Tạo BookImport record
            var bookImport = new BookImport
            {
                BookId = bookId,
                Quantity = quantity,
                ImportDate = DateTime.Now,
                ImportedBy = importedBy,
                CreatedAt = DateTime.Now
            };
            await _bookImportRepository.AddAsync(bookImport);

            // Tạo các BookCopy
            var copies = new List<BookCopy>();
            var existingCopies = await _bookCopyRepository.FindAsync(bc => bc.BookId == bookId);
            var maxCopyNumber = existingCopies.Any() 
                ? existingCopies.Max(bc => int.TryParse(bc.CopyNumber.Replace("B", ""), out var num) ? num : 0)
                : 0;

            for (int i = 1; i <= quantity; i++)
            {
                var copyNumber = $"B{(maxCopyNumber + i):D3}";
                copies.Add(new BookCopy
                {
                    BookId = bookId,
                    CopyNumber = copyNumber,
                    Status = "Available",
                    Condition = "Good",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            await _bookCopyRepository.AddRangeAsync(copies);

            // Cập nhật số lượng sách
            book.TotalCopies += quantity;
            book.AvailableCopies += quantity;
            await _bookRepository.UpdateAsync(book);

            return true;
        }

        public async Task<bool> AddAuthorToBookAsync(int bookId, int authorId, bool isPrimary = true)
        {
            var existing = await _bookAuthorRepository.FirstOrDefaultAsync(
                ba => ba.BookId == bookId && ba.AuthorId == authorId);
            
            if (existing != null) return false;

            var bookAuthor = new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId,
                IsPrimary = isPrimary
            };

            await _bookAuthorRepository.AddAsync(bookAuthor);
            return true;
        }

        public async Task<IEnumerable<BookCopy>> GetAvailableCopiesAsync(int bookId)
        {
            try
            {
                // Query trực tiếp từ DbContext với Include để đảm bảo có dữ liệu
                // Theo schema: Status default là 'Available', các giá trị hợp lệ: 'Available', 'Borrowed', 'Reserved', 'Lost', 'Damaged', 'Maintenance'
                var allCopies = await _context.BookCopies
                    .Include(c => c.Book)
                    .Where(c => c.BookId == bookId)
                    .ToListAsync();
                
                // Log để debug
                System.Diagnostics.Debug.WriteLine($"GetAvailableCopiesAsync - BookId: {bookId}, Total copies: {allCopies.Count}");
                foreach (var copy in allCopies.Take(5))
                {
                    System.Diagnostics.Debug.WriteLine($"  CopyId: {copy.CopyId}, CopyNumber: {copy.CopyNumber}, Status: '{copy.Status}'");
                }
                
                // Lọc trong memory để tránh lỗi với string.IsNullOrEmpty trong LINQ to SQL
                var availableCopies = allCopies.Where(c => 
                {
                    var status = c.Status?.Trim();
                    var isAvailable = string.IsNullOrEmpty(status) || status.Equals("Available", StringComparison.OrdinalIgnoreCase);
                    if (!isAvailable)
                    {
                        System.Diagnostics.Debug.WriteLine($"  Copy {c.CopyId} ({c.CopyNumber}) is not available. Status: '{c.Status}'");
                    }
                    return isAvailable;
                }).ToList();
                
                System.Diagnostics.Debug.WriteLine($"GetAvailableCopiesAsync - Available copies: {availableCopies.Count}");
                
                // Nếu không có bản Available, vẫn trả về tất cả để hiển thị (có thể đang mượn hoặc trạng thái khác)
                // Người dùng sẽ thấy và có thể báo cáo nếu cần
                if (availableCopies.Count == 0 && allCopies.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: No Available copies found, but {allCopies.Count} total copies exist. Returning all copies.");
                    return allCopies;
                }
                
                return availableCopies;
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Error in GetAvailableCopiesAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private static bool IsCopyAvailable(BookCopy copy)
        {
            var status = copy.Status?.Trim();
            // Theo schema, Status default là 'Available'
            // Chấp nhận Available, null, hoặc empty string (cho dữ liệu cũ)
            return string.IsNullOrEmpty(status) || status.Equals("Available", StringComparison.OrdinalIgnoreCase);
        }
    }
}

