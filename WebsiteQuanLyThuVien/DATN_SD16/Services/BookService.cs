using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;

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

        public BookService(
            IBookRepository bookRepository,
            IRepository<BookImport> bookImportRepository,
            IRepository<BookAuthor> bookAuthorRepository,
            IRepository<BookCopy> bookCopyRepository)
        {
            _bookRepository = bookRepository;
            _bookImportRepository = bookImportRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookCopyRepository = bookCopyRepository;
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
            book.UpdatedAt = DateTime.Now;
            await _bookRepository.UpdateAsync(book);
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
    }
}

