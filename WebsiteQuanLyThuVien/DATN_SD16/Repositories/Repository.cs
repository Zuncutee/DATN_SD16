using System.Linq;
using System.Linq.Expressions;
using DATN_SD16.Data;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DATN_SD16.Models.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DATN_SD16.Repositories
{
    /// <summary>
    /// Generic repository implementation
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            // Nếu là BookCopy, detach để tránh conflict với trigger khi update
            if (entity is BookCopy && entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (typeof(T) == typeof(BookCopy) || entity is BookCopy)
            {
                var bookCopy = entity as BookCopy ?? (BookCopy)(object)entity;
                await AddBookCopyAsync(bookCopy);
                return entity;
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (typeof(T) == typeof(BookCopy))
            {
                // Insert từng record một để đảm bảo trigger chạy đúng và tránh conflict
                // Vì Barcode đã được tạo unique trong BookService, không cần transaction riêng
                var copies = entities.Cast<BookCopy>().ToList();
                foreach (var copy in copies)
                {
                    await AddBookCopyAsync(copy);
                }
                return;
            }

            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            // Xử lý đặc biệt cho BookCopy và Borrow vì có trigger trên bảng
            if (typeof(T) == typeof(BookCopy) || entity is BookCopy)
            {
                var bookCopy = entity as BookCopy ?? (BookCopy)(object)entity;
                await UpdateBookCopyAsync(bookCopy);
                return;
            }

            if (typeof(T) == typeof(Borrow) || entity is Borrow)
            {
                var borrow = entity as Borrow ?? (Borrow)(object)entity;
                await UpdateBorrowAsync(borrow);
                return;
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (typeof(T) == typeof(BookCopy))
            {
                foreach (var entity in entities.Cast<BookCopy>())
                {
                    await UpdateBookCopyAsync(entity);
                }
                return;
            }

            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        private async Task UpdateBookCopyAsync(BookCopy bookCopy)
        {
            // Đảm bảo CopyId > 0 để tránh insert thay vì update
            if (bookCopy.CopyId <= 0)
            {
                throw new ArgumentException("CopyId must be greater than 0 for update operation", nameof(bookCopy));
            }

            // Đảm bảo CopyNumber không NULL hoặc empty để tránh vi phạm unique constraint
            if (string.IsNullOrWhiteSpace(bookCopy.CopyNumber))
            {
                throw new ArgumentException("CopyNumber cannot be null or empty", nameof(bookCopy));
            }

            // Detach entity nếu đang được track để tránh conflict
            var entry = _context.Entry(bookCopy);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }

            // Kiểm tra record có tồn tại không bằng cách query
            var exists = await _context.Set<BookCopy>()
                .AsNoTracking()
                .AnyAsync(bc => bc.CopyId == bookCopy.CopyId);

            if (!exists)
            {
                throw new InvalidOperationException($"BookCopy with CopyId {bookCopy.CopyId} does not exist. Cannot update non-existent record.");
            }

            const string sql = @"
UPDATE BookCopies 
SET BookId = @BookId,
    CopyNumber = @CopyNumber,
    Barcode = @Barcode,
    Status = @Status,
    Condition = @Condition,
    PurchaseDate = @PurchaseDate,
    PurchasePrice = @PurchasePrice,
    Notes = @Notes,
    CreatedAt = @CreatedAt,
    UpdatedAt = @UpdatedAt
WHERE CopyId = @CopyId;";

            var parameters = new[]
            {
                new SqlParameter("@BookId", bookCopy.BookId),
                new SqlParameter("@CopyNumber", bookCopy.CopyNumber.Trim()),
                new SqlParameter("@Barcode", (object?)bookCopy.Barcode ?? DBNull.Value),
                new SqlParameter("@Status", bookCopy.Status ?? "Available"),
                new SqlParameter("@Condition", bookCopy.Condition ?? "Good"),
                new SqlParameter("@PurchaseDate", (object?)bookCopy.PurchaseDate ?? DBNull.Value) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@PurchasePrice", (object?)bookCopy.PurchasePrice ?? DBNull.Value) { SqlDbType = SqlDbType.Decimal, Precision = 18, Scale = 2 },
                new SqlParameter("@Notes", (object?)bookCopy.Notes ?? DBNull.Value),
                new SqlParameter("@CreatedAt", bookCopy.CreatedAt) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@UpdatedAt", bookCopy.UpdatedAt) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@CopyId", bookCopy.CopyId)
            };

            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No rows were updated. BookCopy with CopyId {bookCopy.CopyId} may not exist.");
            }
        }

        private async Task AddBookCopyAsync(BookCopy bookCopy)
        {
            // Đảm bảo CopyNumber không NULL hoặc empty để tránh vi phạm unique constraint
            if (string.IsNullOrWhiteSpace(bookCopy.CopyNumber))
            {
                throw new ArgumentException("CopyNumber cannot be null or empty", nameof(bookCopy));
            }

            // Nếu Barcode là NULL hoặc empty, không insert cột Barcode để tránh unique constraint violation
            // Sử dụng dynamic SQL để không include Barcode nếu nó NULL
            var hasBarcode = !string.IsNullOrWhiteSpace(bookCopy.Barcode);
            
            string sql;
            SqlParameter[] parameters;

            if (hasBarcode)
            {
                // Insert với Barcode
                sql = @"
INSERT INTO BookCopies (
    BookId,
    CopyNumber,
    Barcode,
    Status,
    Condition,
    PurchaseDate,
    PurchasePrice,
    Notes,
    CreatedAt,
    UpdatedAt)
VALUES (
    @BookId,
    @CopyNumber,
    @Barcode,
    @Status,
    @Condition,
    @PurchaseDate,
    @PurchasePrice,
    @Notes,
    @CreatedAt,
    @UpdatedAt);";

                parameters = new[]
                {
                    new SqlParameter("@BookId", bookCopy.BookId),
                    new SqlParameter("@CopyNumber", bookCopy.CopyNumber.Trim()),
                    new SqlParameter("@Barcode", bookCopy.Barcode!.Trim()),
                    new SqlParameter("@Status", bookCopy.Status ?? "Available"),
                    new SqlParameter("@Condition", bookCopy.Condition ?? "Good"),
                    new SqlParameter("@PurchaseDate", (object?)bookCopy.PurchaseDate ?? DBNull.Value) { SqlDbType = SqlDbType.DateTime2 },
                    new SqlParameter("@PurchasePrice", (object?)bookCopy.PurchasePrice ?? DBNull.Value) { SqlDbType = SqlDbType.Decimal, Precision = 18, Scale = 2 },
                    new SqlParameter("@Notes", (object?)bookCopy.Notes ?? DBNull.Value),
                    new SqlParameter("@CreatedAt", bookCopy.CreatedAt) { SqlDbType = SqlDbType.DateTime2 },
                    new SqlParameter("@UpdatedAt", bookCopy.UpdatedAt) { SqlDbType = SqlDbType.DateTime2 }
                };
            }
            else
            {
                // Nếu Barcode NULL, tạo Barcode unique tự động dựa trên CopyNumber
                // Format: BC-{CopyNumber} để đảm bảo unique
                var autoBarcode = $"BC-{bookCopy.CopyNumber.Trim()}";
                
                sql = @"
INSERT INTO BookCopies (
    BookId,
    CopyNumber,
    Barcode,
    Status,
    Condition,
    PurchaseDate,
    PurchasePrice,
    Notes,
    CreatedAt,
    UpdatedAt)
VALUES (
    @BookId,
    @CopyNumber,
    @Barcode,
    @Status,
    @Condition,
    @PurchaseDate,
    @PurchasePrice,
    @Notes,
    @CreatedAt,
    @UpdatedAt);";

                parameters = new[]
                {
                    new SqlParameter("@BookId", bookCopy.BookId),
                    new SqlParameter("@CopyNumber", bookCopy.CopyNumber.Trim()),
                    new SqlParameter("@Barcode", autoBarcode),
                    new SqlParameter("@Status", bookCopy.Status ?? "Available"),
                    new SqlParameter("@Condition", bookCopy.Condition ?? "Good"),
                    new SqlParameter("@PurchaseDate", (object?)bookCopy.PurchaseDate ?? DBNull.Value) { SqlDbType = SqlDbType.DateTime2 },
                    new SqlParameter("@PurchasePrice", (object?)bookCopy.PurchasePrice ?? DBNull.Value) { SqlDbType = SqlDbType.Decimal, Precision = 18, Scale = 2 },
                    new SqlParameter("@Notes", (object?)bookCopy.Notes ?? DBNull.Value),
                    new SqlParameter("@CreatedAt", bookCopy.CreatedAt) { SqlDbType = SqlDbType.DateTime2 },
                    new SqlParameter("@UpdatedAt", bookCopy.UpdatedAt) { SqlDbType = SqlDbType.DateTime2 }
                };
            }

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            var inserted = await _context.Set<BookCopy>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.BookId == bookCopy.BookId && c.CopyNumber == bookCopy.CopyNumber);

            if (inserted != null)
            {
                bookCopy.CopyId = inserted.CopyId;
            }
        }

        private async Task UpdateBorrowAsync(Borrow borrow)
        {
            const string sql = @"
UPDATE Borrows
SET BorrowNumber = @BorrowNumber,
    UserId = @UserId,
    CopyId = @CopyId,
    ReservationId = @ReservationId,
    BorrowDate = @BorrowDate,
    DueDate = @DueDate,
    ReturnDate = @ReturnDate,
    Status = @Status,
    FineAmount = @FineAmount,
    FinePaid = @FinePaid,
    ConditionOnBorrow = @ConditionOnBorrow,
    ConditionOnReturn = @ConditionOnReturn,
    Notes = @Notes,
    BorrowedBy = @BorrowedBy,
    ReturnedBy = @ReturnedBy,
    CreatedAt = @CreatedAt,
    UpdatedAt = @UpdatedAt
WHERE BorrowId = @BorrowId;";

            var parameters = new[]
            {
                new SqlParameter("@BorrowNumber", borrow.BorrowNumber),
                new SqlParameter("@UserId", borrow.UserId),
                new SqlParameter("@CopyId", borrow.CopyId),
                new SqlParameter("@ReservationId", (object?)borrow.ReservationId ?? DBNull.Value),
                new SqlParameter("@BorrowDate", borrow.BorrowDate) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@DueDate", borrow.DueDate) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@ReturnDate", (object?)borrow.ReturnDate ?? DBNull.Value) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@Status", borrow.Status ?? "Borrowed"),
                new SqlParameter("@FineAmount", borrow.FineAmount) { SqlDbType = SqlDbType.Decimal, Precision = 18, Scale = 2 },
                new SqlParameter("@FinePaid", borrow.FinePaid) { SqlDbType = SqlDbType.Decimal, Precision = 18, Scale = 2 },
                new SqlParameter("@ConditionOnBorrow", (object?)borrow.ConditionOnBorrow ?? DBNull.Value),
                new SqlParameter("@ConditionOnReturn", (object?)borrow.ConditionOnReturn ?? DBNull.Value),
                new SqlParameter("@Notes", (object?)borrow.Notes ?? DBNull.Value),
                new SqlParameter("@BorrowedBy", borrow.BorrowedBy),
                new SqlParameter("@ReturnedBy", (object?)borrow.ReturnedBy ?? DBNull.Value),
                new SqlParameter("@CreatedAt", borrow.CreatedAt) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@UpdatedAt", borrow.UpdatedAt) { SqlDbType = SqlDbType.DateTime2 },
                new SqlParameter("@BorrowId", borrow.BorrowId)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}

