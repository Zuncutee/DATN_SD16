using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Borrow
    /// </summary>
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BorrowHistory> _borrowHistoryRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly IBookReservationRepository _bookReservationRepository;

        public BorrowService(
            IBorrowRepository borrowRepository,
            IRepository<BookCopy> bookCopyRepository,
            IRepository<Book> bookRepository,
            IRepository<BorrowHistory> borrowHistoryRepository,
            IRepository<SystemSetting> systemSettingRepository,
            IBookReservationRepository bookReservationRepository)
        {
            _borrowRepository = borrowRepository;
            _bookCopyRepository = bookCopyRepository;
            _bookRepository = bookRepository;
            _borrowHistoryRepository = borrowHistoryRepository;
            _systemSettingRepository = systemSettingRepository;
            _bookReservationRepository = bookReservationRepository;
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
            var copy = await _bookCopyRepository.GetByIdAsync(copyId);
            if (copy == null || copy.Status != "Available")
                throw new Exception("Sách không có sẵn để mượn");

            var book = await _bookRepository.GetByIdAsync(copy.BookId);
            if (book == null)
                throw new Exception("Không tìm thấy sách");

            // Lấy cấu hình thời gian mượn
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

            // Cập nhật trạng thái copy
            copy.Status = "Borrowed";
            copy.UpdatedAt = DateTime.Now;
            await _bookCopyRepository.UpdateAsync(copy);

            // Cập nhật số lượng sách
            book.AvailableCopies = Math.Max(0, book.AvailableCopies - 1);
            book.BorrowedCopies += 1;
            await _bookRepository.UpdateAsync(book);

            // Cập nhật reservation nếu có
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

            // Tạo lịch sử
            var history = new BorrowHistory
            {
                BorrowId = borrow.BorrowId,
                UserId = userId,
                CopyId = copyId,
                Action = "Borrow",
                ActionDate = DateTime.Now
            };

            var createdBorrow = await _borrowRepository.AddAsync(borrow);
            history.BorrowId = createdBorrow.BorrowId;
            await _borrowHistoryRepository.AddAsync(history);

            return createdBorrow;
        }

        public async Task<bool> ReturnBookAsync(int borrowId, int returnedBy, string? conditionOnReturn = null)
        {
            var borrow = await _borrowRepository.GetBorrowWithDetailsAsync(borrowId);
            if (borrow == null || borrow.Status != "Borrowed")
                return false;

            var copy = borrow.Copy;
            var book = copy.Book;

            // Cập nhật phiếu mượn
            borrow.ReturnDate = DateTime.Now;
            borrow.Status = "Returned";
            borrow.ReturnedBy = returnedBy;
            borrow.ConditionOnReturn = conditionOnReturn ?? copy.Condition;
            borrow.UpdatedAt = DateTime.Now;

            // Tính phí phạt nếu quá hạn
            if (borrow.ReturnDate > borrow.DueDate)
            {
                borrow.FineAmount = await CalculateFineAsync(borrowId);
            }

            await _borrowRepository.UpdateAsync(borrow);

            // Cập nhật trạng thái copy
            copy.Status = "Available";
            if (!string.IsNullOrEmpty(conditionOnReturn))
            {
                copy.Condition = conditionOnReturn;
            }
            copy.UpdatedAt = DateTime.Now;
            await _bookCopyRepository.UpdateAsync(copy);

            // Cập nhật số lượng sách
            book.AvailableCopies += 1;
            book.BorrowedCopies = Math.Max(0, book.BorrowedCopies - 1);
            await _bookRepository.UpdateAsync(book);

            // Tạo lịch sử
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
            if (borrow == null || borrow.ReturnDate == null)
                return 0;

            if (borrow.ReturnDate <= borrow.DueDate)
                return 0;

            var daysOverdue = (borrow.ReturnDate.Value - borrow.DueDate).Days;
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

            // Tạo lịch sử
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
    }
}

