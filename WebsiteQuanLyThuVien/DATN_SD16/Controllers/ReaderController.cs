using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Models.Entities;
using DATN_SD16.Helpers;
using DATN_SD16.Attributes;
using Microsoft.EntityFrameworkCore;
using DATN_SD16.Data;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller dành cho Độc giả (Reader/User)
    /// </summary>
    [Authorize]
    [AuthorizeRoles("Reader")]
    public class ReaderController : Controller
    {
        private readonly IBorrowService _borrowService;
        private readonly IBookReservationService _reservationService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<LibraryCard> _libraryCardRepository;
        private readonly LibraryDbContext _context;

        public ReaderController(
            IBorrowService borrowService,
            IBookReservationService reservationService,
            IRepository<User> userRepository,
            IRepository<LibraryCard> libraryCardRepository,
            LibraryDbContext context)
        {
            _borrowService = borrowService;
            _reservationService = reservationService;
            _userRepository = userRepository;
            _libraryCardRepository = libraryCardRepository;
            _context = context;
        }

        #region Dashboard
        // GET: Reader/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            
            // Get user info
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get borrowing statistics
            var allBorrows = await _borrowService.GetBorrowsByUserIdAsync(userId);
            var activeBorrows = await _borrowService.GetActiveBorrowsByUserIdAsync(userId);
            var overdueBorrows = allBorrows.Where(b => 
                b.Status == "Borrowed" && b.DueDate < DateTime.Now).ToList();

            // Get reservations
            var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
            var pendingReservations = reservations.Where(r => r.Status == "Pending").ToList();

            // Calculate total fines
            decimal totalFines = 0;
            foreach (var borrow in allBorrows.Where(b => b.Status == "Borrowed"))
            {
                var fine = await _borrowService.CalculateFineAsync(borrow.BorrowId);
                totalFines += fine;
            }

            // Get library card
            var libraryCard = await _context.LibraryCards
                .Where(lc => lc.UserId == userId)
                .OrderByDescending(lc => lc.IssueDate)
                .FirstOrDefaultAsync();

            ViewBag.User = user;
            ViewBag.ActiveBorrowsCount = activeBorrows.Count();
            ViewBag.OverdueBorrowsCount = overdueBorrows.Count;
            ViewBag.PendingReservationsCount = pendingReservations.Count;
            ViewBag.TotalFines = totalFines;
            ViewBag.LibraryCard = libraryCard;

            return View();
        }
        #endregion

        #region Profile Management
        // GET: Reader/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            // Get library card
            var libraryCard = await _context.LibraryCards
                .Where(lc => lc.UserId == userId)
                .OrderByDescending(lc => lc.IssueDate)
                .FirstOrDefaultAsync();

            ViewBag.LibraryCard = libraryCard;

            return View(user);
        }

        // GET: Reader/EditProfile
        public async Task<IActionResult> EditProfile()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Reader/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(User model)
        {
            try
            {
                var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var user = await _userRepository.GetByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                // Update only allowed fields
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.UpdatedAt = DateTime.Now;

                await _userRepository.UpdateAsync(user);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
                }

                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        // GET: Reader/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Reader/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }

                if (newPassword != confirmPassword)
                {
                    throw new Exception("Mật khẩu mới không khớp");
                }

                if (newPassword.Length < 6)
                {
                    throw new Exception("Mật khẩu phải có ít nhất 6 ký tự");
                }

                var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var user = await _userRepository.GetByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                {
                    throw new Exception("Mật khẩu hiện tại không đúng");
                }

                // Hash and update new password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdatedAt = DateTime.Now;

                await _userRepository.UpdateAsync(user);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
                }

                TempData["Success"] = "Đổi mật khẩu thành công!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return View();
            }
        }
        #endregion

        #region Helper Methods
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   Request.Headers["Accept"].ToString().Contains("application/json");
        }
        #endregion
    }
}

