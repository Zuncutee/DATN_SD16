using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Attributes;
using DATN_SD16.Helpers;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý đặt sách
    /// </summary>
    [Authorize]
    public class BookReservationsController : Controller
    {
        private readonly IBookReservationService _reservationService;

        public BookReservationsController(IBookReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: BookReservations
        public async Task<IActionResult> Index()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var reservations = await _reservationService.GetReservationsByUserIdAsync(userId.Value);
            return View(reservations);
        }

        // GET: BookReservations/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // POST: BookReservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId)
        {
            try
            {
                var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _reservationService.CreateReservationAsync(userId.Value, bookId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Details", "Books", new { id = bookId });
            }
        }

        // POST: BookReservations/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _reservationService.CancelReservationAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // POST: BookReservations/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Librarian", "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var approvedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _reservationService.ApproveReservationAsync(id, approvedBy.Value);
                return RedirectToAction("Pending", "Admin");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // POST: BookReservations/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Librarian", "Admin")]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            try
            {
                var rejectedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _reservationService.RejectReservationAsync(id, rejectedBy.Value, reason);
                return RedirectToAction("Pending", "Admin");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}

