using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;

namespace DATN_SD16.Services
{
    // Service implementation cho BookReservation
    public class BookReservationService : IBookReservationService
    {
        private readonly IBookReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly IRepository<LibraryCard> _libraryCardRepository;

        public BookReservationService(
            IBookReservationRepository reservationRepository,
            IBookRepository bookRepository,
            IRepository<SystemSetting> systemSettingRepository,
            IRepository<LibraryCard> libraryCardRepository)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _systemSettingRepository = systemSettingRepository;
            _libraryCardRepository = libraryCardRepository;
        }

        public async Task<BookReservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepository.GetByIdAsync(reservationId);
        }

        public async Task<BookReservation?> GetReservationWithDetailsAsync(int reservationId)
        {
            return await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
        }

        public async Task<IEnumerable<BookReservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _reservationRepository.GetReservationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<BookReservation>> GetPendingReservationsAsync()
        {
            return await _reservationRepository.GetPendingReservationsAsync();
        }

        public async Task<BookReservation> CreateReservationAsync(int userId, int bookId)
        {
            // Kiểm tra trạng thái thẻ thư viện
            var libraryCard = await _libraryCardRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            
            if (libraryCard == null)
            {
                throw new Exception("Bạn chưa có thẻ thư viện. Vui lòng liên hệ thủ thư để được cấp thẻ.");
            }

            if (libraryCard.Status != "Active")
            {
                // Thông báo cụ thể tùy trạng thái
                string message = libraryCard.Status switch
                {
                    "Suspended" => "Thẻ thư viện của bạn đang bị tạm ngưng.",
                    "Expired" => "Thẻ thư viện của bạn đã hết hạn.",
                    "Cancelled" => "Thẻ thư viện của bạn đã bị hủy.",
                    _ => $"Thẻ thư viện không khả dụng (Trạng thái: {libraryCard.Status})."
                };
                throw new Exception(message + " Không thể đặt sách.");
            }

            if (libraryCard.ExpiryDate != null && libraryCard.ExpiryDate < DateTime.Now)
            {
                throw new Exception("Thẻ thư viện của bạn đã hết hạn. Vui lòng gia hạn thẻ.");
            }

            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new Exception("Không tìm thấy sách");

            if (book.AvailableCopies <= 0)
                throw new Exception("Sách hiện không có sẵn");

            var existingReservation = await _reservationRepository.FirstOrDefaultAsync(
                r => r.UserId == userId && r.BookId == bookId && r.Status == "Pending");
            
            if (existingReservation != null)
            {
                if (existingReservation.ExpiryDate != null && existingReservation.ExpiryDate < DateTime.Now)
                {
                    // Reservation đã hết hạn, cập nhật thành Cancelled và cho phép đặt mới
                    existingReservation.Status = "Cancelled";
                    existingReservation.UpdatedAt = DateTime.Now;
                    await _reservationRepository.UpdateAsync(existingReservation);
                }
                else
                {
                    throw new Exception("Bạn đã đặt sách này rồi");
                }
            }

            var expiryDaysSetting = await _systemSettingRepository.FirstOrDefaultAsync(
                s => s.SettingKey == "ReservationExpiryDays");
            var expiryDays = expiryDaysSetting != null 
                ? int.Parse(expiryDaysSetting.SettingValue) 
                : 3;

            var reservation = new BookReservation
            {
                UserId = userId,
                BookId = bookId,
                ReservationDate = DateTime.Now,
                Status = "Pending",
                ExpiryDate = DateTime.Now.AddDays(expiryDays),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return await _reservationRepository.AddAsync(reservation);
        }

        public async Task<bool> ApproveReservationAsync(int reservationId, int approvedBy)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "Pending")
                return false;

            reservation.Status = "Approved";
            reservation.ApprovedBy = approvedBy;
            reservation.ApprovedAt = DateTime.Now;
            reservation.UpdatedAt = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        public async Task<bool> RejectReservationAsync(int reservationId, int rejectedBy, string reason)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "Pending")
                return false;

            reservation.Status = "Rejected";
            reservation.ApprovedBy = rejectedBy;
            reservation.ApprovedAt = DateTime.Now;
            reservation.RejectionReason = reason;
            reservation.UpdatedAt = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "Pending")
                return false;

            reservation.Status = "Cancelled";
            reservation.UpdatedAt = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }
    }
}

