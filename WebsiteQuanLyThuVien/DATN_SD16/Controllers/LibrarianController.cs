using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Models.Entities;
using DATN_SD16.Helpers;
using DATN_SD16.Attributes;
using DATN_SD16.Data;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý các chức năng dành cho Thủ Thư (Librarian)
    /// </summary>
    [Authorize]
    [AuthorizeRoles("Librarian", "Admin")]
    public class LibrarianController : Controller
    {
        private readonly IBorrowService _borrowService;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookReservationService _reservationService;
        private readonly IBookReservationRepository _bookReservationRepository;
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<LibraryCard> _libraryCardRepository;
        private readonly IRepository<BookDamage> _bookDamageRepository;
        private readonly IRepository<InventoryCheck> _inventoryCheckRepository;
        private readonly IRepository<InventoryCheckDetail> _inventoryCheckDetailRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly LibraryDbContext _context;

        public LibrarianController(
            IBorrowService borrowService,
            IBorrowRepository borrowRepository,
            IBookReservationService reservationService,
            IBookReservationRepository bookReservationRepository,
            IRepository<BookCopy> bookCopyRepository,
            IRepository<Book> bookRepository,
            IRepository<User> userRepository,
            IRepository<LibraryCard> libraryCardRepository,
            IRepository<BookDamage> bookDamageRepository,
            IRepository<InventoryCheck> inventoryCheckRepository,
            IRepository<InventoryCheckDetail> inventoryCheckDetailRepository,
            IRepository<SystemSetting> systemSettingRepository,
            LibraryDbContext context)
        {
            _borrowService = borrowService;
            _borrowRepository = borrowRepository;
            _reservationService = reservationService;
            _bookReservationRepository = bookReservationRepository;
            _bookCopyRepository = bookCopyRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _libraryCardRepository = libraryCardRepository;
            _bookDamageRepository = bookDamageRepository;
            _inventoryCheckRepository = inventoryCheckRepository;
            _inventoryCheckDetailRepository = inventoryCheckDetailRepository;
            _systemSettingRepository = systemSettingRepository;
            _context = context;
        }

        #region Dashboard
        // GET: Librarian/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var allBorrows = await _borrowRepository.GetAllAsync();
            var overdueBorrows = await _borrowRepository.GetOverdueBorrowsAsync();
            var pendingReservations = await _reservationService.GetPendingReservationsAsync();
            var activeBorrows = allBorrows.Where(b => b.Status == "Borrowed").ToList();

            ViewBag.TotalBorrows = allBorrows.Count();
            ViewBag.ActiveBorrows = activeBorrows.Count();
            ViewBag.OverdueBorrows = overdueBorrows.Count();
            ViewBag.PendingReservations = pendingReservations.Count();
            return View();
        }
        #endregion

        #region Quản lý Mượn - Trả
        // GET: Librarian/Borrows
        public async Task<IActionResult> Borrows(string? status)
        {
            IEnumerable<Borrow> borrows;
            if (string.IsNullOrEmpty(status) || status == "All")
            {
                borrows = await _borrowRepository.GetAllAsync();
            }
            else
            {
                borrows = await _borrowRepository.FindAsync(b => b.Status == status);
            }

            ViewBag.Status = status ?? "All";
            return View(borrows);
        }

        // GET: Librarian/Borrows/Create
        public async Task<IActionResult> CreateBorrow()
        {
            ViewBag.Users = await _userRepository.GetAllAsync();
            ViewBag.Books = await _bookRepository.GetAllAsync();
            return View();
        }

        // GET: Librarian/GetReservations?userId=1&bookId=2
        [HttpGet]
        public async Task<IActionResult> GetReservations(int? userId, int? bookId)
        {
            try
            {
                if (!userId.HasValue)
                {
                    return Json(new List<object>());
                }

                var allReservations = await _bookReservationRepository.GetAllAsync();
                
                var filtered = allReservations.Where(r => 
                    r.UserId == userId.Value &&
                    (r.Status == "Pending" || r.Status == "Approved"));
                
                if (bookId.HasValue)
                {
                    filtered = filtered.Where(r => r.BookId == bookId.Value);
                }

                var result = filtered.Select(r => new
                {
                    reservationId = r.ReservationId,
                    bookId = r.BookId,
                    bookTitle = r.Book?.Title ?? "N/A",
                    reservationDate = r.ReservationDate.ToString("dd/MM/yyyy"),
                    expiryDate = r.ExpiryDate?.ToString("dd/MM/yyyy") ?? "N/A",
                    status = r.Status
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: Librarian/Borrows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBorrow(int userId, int copyId, int? reservationId)
        {
            try
            {
                var borrowedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var borrow = await _borrowService.CreateBorrowAsync(userId, copyId, borrowedBy, reservationId);
                
                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo phiếu mượn thành công!" });
                }
                
                TempData["Success"] = "Tạo phiếu mượn thành công!";
                return RedirectToAction(nameof(Borrows));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }
                
                TempData["Error"] = ex.Message;
                ViewBag.Users = await _userRepository.GetAllAsync();
                ViewBag.Books = await _bookRepository.GetAllAsync();
                return View();
            }
        }

        // GET: Librarian/Borrows/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _borrowService.GetBorrowWithDetailsAsync(id.Value);
            if (borrow == null)
            {
                return NotFound();
            }

            // Tính phí phạt nếu có
            var fineAmount = await _borrowService.CalculateFineAsync(id.Value);
            ViewBag.FineAmount = fineAmount;

            return View(borrow);
        }

        // POST: Librarian/Borrows/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, string? conditionOnReturn, decimal? finePaid)
        {
            try
            {
                var returnedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _borrowService.ReturnBookAsync(id, returnedBy, conditionOnReturn);

                if (finePaid.HasValue && finePaid.Value > 0)
                {
                    var borrow = await _borrowRepository.GetByIdAsync(id);
                    if (borrow != null)
                    {
                        borrow.FinePaid = finePaid.Value;
                        await _borrowRepository.UpdateAsync(borrow);
                    }
                }

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Trả sách thành công!" });
                }

                TempData["Success"] = "Trả sách thành công!";
                return RedirectToAction(nameof(Borrows));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Return), new { id });
            }
        }

        // GET: Librarian/Borrows/Details/5
        public async Task<IActionResult> BorrowDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _borrowService.GetBorrowWithDetailsAsync(id.Value);
            if (borrow == null)
            {
                return NotFound();
            }

            var fineAmount = await _borrowService.CalculateFineAsync(id.Value);
            ViewBag.FineAmount = fineAmount;

            return View(borrow);
        }

        // GET: Librarian/Borrows/Overdue
        public async Task<IActionResult> OverdueBorrows()
        {
            var overdueBorrows = await _borrowRepository.GetOverdueBorrowsAsync();
            return View(overdueBorrows);
        }
        #endregion

        #region Xử lý Đặt sách Online
        // GET: Librarian/Reservations
        public async Task<IActionResult> Reservations(string? status)
        {
            IEnumerable<BookReservation> reservations;
            if (string.IsNullOrEmpty(status) || status == "All")
            {
                reservations = await _reservationService.GetPendingReservationsAsync();
            }
            else if (status == "Pending")
            {
                reservations = await _reservationService.GetPendingReservationsAsync();
            }
            else
            {
                var allReservations = await _bookReservationRepository.GetAllAsync();
                reservations = allReservations.Where(r => r.Status == status);
            }

            ViewBag.Status = status ?? "Pending";
            return View(reservations);
        }

        // GET: Librarian/Reservations/Details/5
        public async Task<IActionResult> ReservationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationWithDetailsAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Librarian/Reservations/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveReservation(int id)
        {
            try
            {
                var approvedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _reservationService.ApproveReservationAsync(id, approvedBy);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Duyệt đặt sách thành công!" });
                }

                TempData["Success"] = "Duyệt đặt sách thành công!";
                return RedirectToAction(nameof(Reservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ReservationDetails), new { id });
            }
        }

        // POST: Librarian/Reservations/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectReservation(int id, string reason)
        {
            try
            {
                var rejectedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _reservationService.RejectReservationAsync(id, rejectedBy, reason);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Từ chối đặt sách thành công!" });
                }

                TempData["Success"] = "Từ chối đặt sách thành công!";
                return RedirectToAction(nameof(Reservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ReservationDetails), new { id });
            }
        }
        #endregion

        #region Quản lý Độc giả
        // GET: Librarian/Readers
        public async Task<IActionResult> Readers(string? search)
        {
            IEnumerable<User> users;
            if (!string.IsNullOrEmpty(search))
            {
                users = await _userRepository.FindAsync(u => 
                    u.FullName.Contains(search) || 
                    u.Username.Contains(search) || 
                    u.Email.Contains(search));
            }
            else
            {
                users = await _userRepository.GetAllAsync();
            }

            ViewBag.Search = search;
            return View(users);
        }

        // GET: Librarian/Readers/Details/5
        public async Task<IActionResult> ReaderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            var borrows = await _borrowService.GetBorrowsByUserIdAsync(id.Value);
            var activeBorrows = await _borrowService.GetActiveBorrowsByUserIdAsync(id.Value);
            var overdueBorrows = borrows.Where(b => b.Status == "Overdue" || (b.DueDate < DateTime.Now && b.Status == "Borrowed"));

            ViewBag.Borrows = borrows;
            ViewBag.ActiveBorrows = activeBorrows;
            ViewBag.OverdueBorrows = overdueBorrows;
            ViewBag.LibraryCard = await _libraryCardRepository.FirstOrDefaultAsync(c => c.UserId == id.Value);

            return View(user);
        }

        // GET: Librarian/LibraryCards
        public async Task<IActionResult> LibraryCards()
        {
            var cards = await _libraryCardRepository.GetAllAsync();
            return View(cards);
        }

        // GET: Librarian/LibraryCards/Create
        public async Task<IActionResult> CreateLibraryCard()
        {
            var users = await _userRepository.GetAllAsync();
            var allCards = await _libraryCardRepository.GetAllAsync();
            var usersWithCardIds = allCards.Select(c => c.UserId).ToHashSet();
            var usersWithoutCard = users.Where(u => !usersWithCardIds.Contains(u.UserId)).ToList();
            ViewBag.Users = usersWithoutCard;
            return View();
        }

        // POST: Librarian/LibraryCards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLibraryCard(int userId, DateTime? expiryDate)
        {
            try
            {
                var existingCard = await _libraryCardRepository.FirstOrDefaultAsync(c => c.UserId == userId);
                if (existingCard != null)
                {
                    if (IsAjaxRequest())
                    {
                        return Json(new { success = false, message = "Độc giả này đã có thẻ thư viện" });
                    }
                    TempData["Error"] = "Độc giả này đã có thẻ thư viện";
                    return RedirectToAction(nameof(CreateLibraryCard));
                }

                var createdBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var cardNumber = $"TH{DateTime.Now:yyyyMMdd}{userId:D4}";

                var card = new LibraryCard
                {
                    UserId = userId,
                    CardNumber = cardNumber,
                    IssueDate = DateTime.Now,
                    ExpiryDate = expiryDate,
                    Status = "Active",
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _libraryCardRepository.AddAsync(card);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo thẻ thư viện thành công!" });
                }

                TempData["Success"] = "Tạo thẻ thư viện thành công!";
                return RedirectToAction(nameof(LibraryCards));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(CreateLibraryCard));
            }
        }

        // POST: Librarian/LibraryCards/Suspend/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuspendLibraryCard(int id)
        {
            try
            {
                var card = await _libraryCardRepository.GetByIdAsync(id);
                if (card == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thẻ thư viện" });
                }

                card.Status = "Suspended";
                card.UpdatedAt = DateTime.Now;
                await _libraryCardRepository.UpdateAsync(card);

                return Json(new { success = true, message = "Đã tạm ngưng thẻ thư viện" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Librarian/LibraryCards/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateLibraryCard(int id)
        {
            try
            {
                var card = await _libraryCardRepository.GetByIdAsync(id);
                if (card == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thẻ thư viện" });
                }

                card.Status = "Active";
                card.UpdatedAt = DateTime.Now;
                await _libraryCardRepository.UpdateAsync(card);

                return Json(new { success = true, message = "Đã kích hoạt thẻ thư viện" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Quản lý Sách hỏng/mất
        // GET: Librarian/BookDamages
        public async Task<IActionResult> BookDamages(string? status)
        {
            IQueryable<BookDamage> query = _context.BookDamages
                .Include(d => d.Copy)
                    .ThenInclude(c => c.Book)
                .Include(d => d.ReportedByUser)
                .Include(d => d.ProcessedByUser);

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(d => d.Status == status);
            }

            var damages = await query.ToListAsync();

            ViewBag.Status = status ?? "All";
            return View(damages);
        }

        // GET: Librarian/BookDamages/Create
        public async Task<IActionResult> CreateBookDamage()
        {
            ViewBag.BookCopies = await _bookCopyRepository.GetAllAsync();
            return View();
        }

        // POST: Librarian/BookDamages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBookDamage(BookDamage damage)
        {
            try
            {
                var reportedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                
                damage.ReportedBy = reportedBy;
                damage.DamageDate = DateTime.Now;
                damage.Status = "Reported";
                damage.CreatedAt = DateTime.Now;
                damage.UpdatedAt = DateTime.Now;

                await _bookDamageRepository.AddAsync(damage);
                try
                {
                    var copy = await _bookCopyRepository.GetByIdAsync(damage.CopyId);
                    if (copy != null)
                    {
                        if (damage.DamageType == "Lost")
                        {
                            copy.Status = "Lost";
                        }
                        else if (damage.DamageType == "Damaged")
                        {
                            copy.Status = "Damaged";
                            copy.Condition = "Damaged";
                        }
                        else if (damage.DamageType == "Missing")
                        {
                            copy.Status = "Lost";
                        }
                        copy.UpdatedAt = DateTime.Now;
                        await _bookCopyRepository.UpdateAsync(copy);
                        
                        // Không cần cập nhật Books thủ công vì trigger TRG_BookCopies_UpdateAvailableCopies_Update sẽ tự động cập nhật
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating BookCopy: {ex.Message}");
                }

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Báo cáo sách hỏng/mất thành công!" });
                }

                TempData["Success"] = "Báo cáo sách hỏng/mất thành công!";
                return RedirectToAction(nameof(BookDamages));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                ViewBag.BookCopies = await _bookCopyRepository.GetAllAsync();
                return View(damage);
            }
        }

        // GET: Librarian/BookDamages/Details/5
        public async Task<IActionResult> BookDamageDetails(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var damage = await _context.BookDamages
                .Include(d => d.Copy)
                    .ThenInclude(c => c.Book)
                .Include(d => d.ReportedByUser)
                .Include(d => d.ProcessedByUser)
                .FirstOrDefaultAsync(d => d.DamageId == id);

            if (damage == null)
            {
                return NotFound();
            }

            return View(damage);
        }
        #endregion

        #region Kiểm kê Sách
        // GET: Librarian/InventoryChecks
        public async Task<IActionResult> InventoryChecks(string? status)
        {
            IEnumerable<InventoryCheck> checks;
            if (string.IsNullOrEmpty(status) || status == "All")
            {
                checks = await _inventoryCheckRepository.GetAllAsync();
            }
            else
            {
                checks = await _inventoryCheckRepository.FindAsync(c => c.Status == status);
            }

            ViewBag.Status = status ?? "All";
            return View(checks);
        }

        // GET: Librarian/InventoryChecks/Create
        public async Task<IActionResult> CreateInventoryCheck()
        {
            return View();
        }

        // POST: Librarian/InventoryChecks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInventoryCheck(InventoryCheck check)
        {
            try
            {
                var checkedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                
                check.CheckNumber = $"KK{DateTime.Now:yyyyMMddHHmmss}";
                check.CheckDate = DateTime.Now;
                check.CheckedBy = checkedBy;
                check.Status = "InProgress";
                check.CreatedAt = DateTime.Now;
                check.UpdatedAt = DateTime.Now;

                await _inventoryCheckRepository.AddAsync(check);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo phiếu kiểm kê thành công!", checkId = check.CheckId });
                }

                TempData["Success"] = "Tạo phiếu kiểm kê thành công!";
                return RedirectToAction(nameof(InventoryCheckDetails), new { id = check.CheckId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return View(check);
            }
        }

        // GET: Librarian/InventoryChecks/Details/5
        public async Task<IActionResult> InventoryCheckDetails(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var check = await _inventoryCheckRepository.GetByIdAsync(id);
            if (check == null)
            {
                return NotFound();
            }

            var details = await _inventoryCheckDetailRepository.FindAsync(d => d.CheckId == id);
            ViewBag.Details = details;
            ViewBag.BookCopies = await _bookCopyRepository.GetAllAsync();

            return View(check);
        }

        // POST: Librarian/InventoryChecks/AddDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInventoryCheckDetail(int checkId, int copyId, string actualStatus, string? notes)
        {
            try
            {
                var copy = await _bookCopyRepository.GetByIdAsync(copyId);
                if (copy == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bản sách" });
                }

                var detail = new InventoryCheckDetail
                {
                    CheckId = checkId,
                    CopyId = copyId,
                    ExpectedStatus = copy.Status,
                    ActualStatus = actualStatus,
                    IsMatched = copy.Status == actualStatus,
                    Notes = notes
                };

                await _inventoryCheckDetailRepository.AddAsync(detail);

                return Json(new { success = true, message = "Thêm chi tiết kiểm kê thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Librarian/InventoryChecks/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteInventoryCheck(int id)
        {
            try
            {
                var check = await _inventoryCheckRepository.GetByIdAsync(id);
                if (check == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu kiểm kê" });
                }

                check.Status = "Completed";
                check.UpdatedAt = DateTime.Now;
                await _inventoryCheckRepository.UpdateAsync(check);
                var details = await _inventoryCheckDetailRepository.FindAsync(d => d.CheckId == id);
                foreach (var detail in details)
                {
                    if (!detail.IsMatched && detail.ActualStatus != null)
                    {
                        var copy = await _bookCopyRepository.GetByIdAsync(detail.CopyId);
                        if (copy != null)
                        {
                            copy.Status = detail.ActualStatus;
                            copy.UpdatedAt = DateTime.Now;
                            await _bookCopyRepository.UpdateAsync(copy);
                        }
                    }
                }

                return Json(new { success = true, message = "Hoàn thành kiểm kê thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Helper Methods
        private bool IsAjaxRequest()
        {
            if (Request?.Headers.TryGetValue("X-Requested-With", out var requestedWith) == true &&
                string.Equals(requestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (Request?.Headers.TryGetValue("Accept", out var acceptValues) == true)
            {
                return acceptValues.Any(value => value != null &&
                    value.Contains("application/json", StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }
        #endregion
    }
}

