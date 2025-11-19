using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Repositories
{
    /// <summary>
    /// Repository implementation cho Book
    /// </summary>
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Location)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookCopies)
                .FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string? title, string? author, int? categoryId, bool? availableOnly)
        {
            var query = _dbSet
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.BookAuthors.Any(ba => ba.Author.AuthorName.Contains(author)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (availableOnly == true)
            {
                query = query.Where(b => b.AvailableCopies > 0);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _dbSet
                .Where(b => b.AvailableCopies > 0 && b.Status == "Active")
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetMostBorrowedBooksAsync(int top = 10)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Include(b => b.BookCopies)
                    .ThenInclude(bc => bc.Borrows)
                .OrderByDescending(b => b.BookCopies
                    .SelectMany(bc => bc.Borrows)
                    .Count())
                .Take(top)
                .ToListAsync();
        }
    }
}

