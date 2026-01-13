using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace DATN_SD16.Services
{
    // Service implementation cho Borrow
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BorrowHistory> _borrowHistoryRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly IBookReservationRepository _bookReservationRepository;
        private readonly LibraryDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly ILogger<BorrowService>? _logger;

        public BorrowService(
            IBorrowRepository borrowRepository,
            IRepository<BookCopy> bookCopyRepository,
            IRepository<Book> bookRepository,
            IRepository<BorrowHistory> borrowHistoryRepository,
            IRepository<SystemSetting> systemSettingRepository,
            IBookReservationRepository bookReservationRepository,
            LibraryDbContext context,
            INotificationService notificationService,
            ILogger<BorrowService>? logger = null)
        {
            _borrowRepository = borrowRepository;
            _bookCopyRepository = bookCopyRepository;
            _bookRepository = bookRepository;
            _borrowHistoryRepository = borrowHistoryRepository;
            _systemSettingRepository = systemSettingRepository;
            _bookReservationRepository = bookReservationRepository;
            _context = context;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int borrowId)
        {
            return await _borrowRepository.GetByIdAsync(borrowId);
        }

        public async Task<Borrow?> GetBorrowWithDetailsAsync(int borrowId)
        {
            return await _borrowRepository.GetBorrowWithDetailsAsync(borrowId);
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId)
        {
            return await _borrowRepository.GetBorrowsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync()
        {
            return await _borrowRepository.GetOverdueBorrowsAsync();
        }

        public async Task<IEnumerable<Borrow>> GetActiveBorrowsByUserIdAsync(int userId)
        {
            return await _borrowRepository.GetActiveBorrowsByUserIdAsync(userId);
        }

        public async Task<Borrow> CreateBorrowAsync(int userId, int copyId, int borrowedBy, int? reservationId = null)
        {
            // Sử dụng transaction với isolation level Serializable để tránh race condition
            // Khi 4 người cùng mượn 3 cuốn sách, chỉ 3 người được mượn, 1 người sẽ bị từ chối
            //using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                // Lock row BookCopy để tránh đọc đồng thời và đảm bảo atomic update
                var copy = await _context.BookCopies
                    .FromSqlRaw("SELECT * FROM BookCopies WITH (UPDLOCK, ROWLOCK) WHERE CopyId = {0}", copyId)
                    .FirstOrDefaultAsync();

                if (copy == null)
                {
                    //await transaction.RollbackAsync();
                    throw new Exception("Không tìm thấy bản sách");
                }

                // Kiểm tra status sau khi lock
                if (!IsCopyAvailable(copy.Status))
                {
                    //await transaction.RollbackAsync();
                    throw new Exception("Sách không có sẵn để mượn");
                }

                var book = await _bookRepository.GetByIdAsync(copy.BookId);
                if (book == null)
                {
                    //await transaction.RollbackAsync();
                    throw new Exception("Không tìm thấy sách");
                }

                var maxBorrowDaysSetting = await _systemSettingRepository.FirstOrDefaultAsync(
                    s => s.SettingKey == "MaxBorrowDays");
                var maxBorrowDays = maxBorrowDaysSetting != null 
                    ? int.Parse(maxBorrowDaysSetting.SettingValue) 
                    : 14;

                var borrowNumber = $"BR{DateTime.Now:yyyyMMddHHmmss}{userId}";
                var borrow = new Borrow
                {
                    BorrowNumber = borrowNumber,
                    UserId = userId,
                    CopyId = copyId,
                    ReservationId = reservationId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(maxBorrowDays),
                    Status = "Borrowed",
                    BorrowedBy = borrowedBy,
                    ConditionOnBorrow = copy.Condition,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Update status bằng SQL với điều kiện WHERE Status = 'Available'
                // Đảm bảo chỉ update được nếu vẫn còn Available (atomic operation)
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    @"UPDATE BookCopies 
                      SET Status = {0}, 
                          UpdatedAt = {1}
                      WHERE CopyId = {2} 
                      AND Status = {3}",
                    "Borrowed", DateTime.Now, copyId, "Available");

                // Nếu không update được (rowsAffected = 0), có nghĩa là sách đã bị mượn bởi người khác
                if (rowsAffected == 0)
                {
                    // Lấy thông tin sách để thông báo chi tiết
                    var bookTitle = book?.Title ?? "Sách";
                    var copyNumber = copy?.CopyNumber ?? "";
                    var bookId = copy?.BookId ?? 0;
                    
                    // Log thông tin người bị từ chối
                    _logger?.LogWarning(
                        "User {UserId} bị từ chối mượn sách. BookId: {BookId}, CopyId: {CopyId}, BookTitle: {BookTitle}, CopyNumber: {CopyNumber}, Time: {Time}",
                        userId, bookId, copyId, bookTitle, copyNumber, DateTime.Now);

                    // Tạo notification cho người bị từ chối
                    try
                    {
                        await _notificationService.CreateNotificationAsync(
                            userId,
                            "System",
                            "Mượn sách không thành công",
                            $"Rất tiếc, bạn không thể mượn sách \"{bookTitle}\" (Bản số: {copyNumber}). Sách đã được người khác mượn trước đó.",
                            relatedBorrowId: null,
                            relatedReservationId: reservationId);
                    }
                    catch (Exception notifEx)
                    {
                        // Log lỗi tạo notification nhưng không throw để tránh che giấu lỗi chính
                        _logger?.LogError(notifEx, "Lỗi khi tạo notification cho user {UserId}", userId);
                    }

                    //await transaction.RollbackAsync();
                    throw new Exception($"Sách \"{bookTitle}\" (Bản số: {copyNumber}) không còn sẵn sàng để mượn. Sách đã được người khác mượn trước đó. Bạn đã nhận được thông báo trong hệ thống.");
                }

                if (reservationId.HasValue)
                {
                    var reservation = await _bookReservationRepository.GetByIdAsync(reservationId.Value);
                    if (reservation != null)
                    {
                        reservation.Status = "Completed";
                        reservation.UpdatedAt = DateTime.Now;
                        await _bookReservationRepository.UpdateAsync(reservation);
                    }
                }

                var createdBorrow = await _borrowRepository.AddAsync(borrow);

                var history = new BorrowHistory
                {
                    BorrowId = createdBorrow.BorrowId,
                    UserId = userId,
                    CopyId = copyId,
                    Action = "Borrow",
                    ActionDate = DateTime.Now
                };
                await _borrowHistoryRepository.AddAsync(history);

                // Commit transaction nếu tất cả thành công
                //await transaction.CommitAsync();
                return createdBorrow;
            }
            catch (Exception)
            {
                // Rollback transaction nếu có lỗi
                //await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ReturnBookAsync(int borrowId, int returnedBy, string? conditionOnReturn = null)
        {
            var borrow = await _borrowRepository.GetBorrowWithDetailsAsync(borrowId);
            if (borrow == null || borrow.Status != "Borrowed")
                return false;

            var copy = borrow.Copy;
            var book = copy.Book;

            borrow.ReturnDate = DateTime.Now;
            borrow.Status = "Returned";
            borrow.ReturnedBy = returnedBy;
            borrow.ConditionOnReturn = conditionOnReturn ?? copy.Condition;
            borrow.UpdatedAt = DateTime.Now;

            // Tính phí phạt nếu quá hạn
            if (borrow.ReturnDate > borrow.DueDate)
            {
                borrow.FineAmount = await CalculateFineAsync(borrow);
            }

            await _borrowRepository.UpdateAsync(borrow);

            copy.Status = "Available";
            if (!string.IsNullOrEmpty(conditionOnReturn))
            {
                copy.Condition = conditionOnReturn;
            }
            copy.UpdatedAt = DateTime.Now;
            await _bookCopyRepository.UpdateAsync(copy);

            book.AvailableCopies += 1;
            book.BorrowedCopies = Math.Max(0, book.BorrowedCopies - 1);
            await _bookRepository.UpdateAsync(book);

            var history = new BorrowHistory
            {
                BorrowId = borrowId,
                UserId = borrow.UserId,
                CopyId = copy.CopyId,
                Action = "Return",
                ActionDate = DateTime.Now
            };
            await _borrowHistoryRepository.AddAsync(history);

            return true;
        }

        public async Task<decimal> CalculateFineAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetBorrowWithDetailsAsync(borrowId);
            if (borrow == null)
                return 0;

            return await CalculateFineAsync(borrow);
        }

        public async Task<decimal> CalculateFineAsync(Borrow borrow)
        {
            var returnDate = borrow.ReturnDate ?? DateTime.Now;

            if (returnDate <= borrow.DueDate)
                return 0;

            var daysOverdue = (returnDate - borrow.DueDate).Days;
            
            if (daysOverdue <= 0) return 0;

            var finePerDaySetting = await _systemSettingRepository.FirstOrDefaultAsync(
                s => s.SettingKey == "FinePerDay");
            
            var finePerDay = finePerDaySetting != null 
                ? decimal.Parse(finePerDaySetting.SettingValue) 
                : 5000;

            return daysOverdue * finePerDay;
        }

        public async Task<bool> RenewBorrowAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null || borrow.Status != "Borrowed")
                return false;

            var maxRenewDaysSetting = await _systemSettingRepository.FirstOrDefaultAsync(
                s => s.SettingKey == "MaxRenewDays");
            var maxRenewDays = maxRenewDaysSetting != null 
                ? int.Parse(maxRenewDaysSetting.SettingValue) 
                : 7;

            borrow.DueDate = borrow.DueDate.AddDays(maxRenewDays);
            borrow.UpdatedAt = DateTime.Now;
            await _borrowRepository.UpdateAsync(borrow);

            var history = new BorrowHistory
            {
                BorrowId = borrowId,
                UserId = borrow.UserId,
                CopyId = borrow.CopyId,
                Action = "Renew",
                ActionDate = DateTime.Now
            };
            await _borrowHistoryRepository.AddAsync(history);

            return true;
        }

        private static bool IsCopyAvailable(string? status)
        {
            var normalized = status?.Trim();
            return string.IsNullOrEmpty(normalized) || normalized.Equals("Available", StringComparison.OrdinalIgnoreCase);
        }
    }
}

