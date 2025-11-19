using Microsoft.AspNetCore.Mvc;
using DATN_SD16.Services.Interfaces;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý đặt sách
    /// </summary>
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
            // TODO: Lấy userId từ session/claims
            int userId = 1; // Tạm thời hardcode
            var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
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
                // TODO: Lấy userId từ session/claims
                int userId = 1; // Tạm thời hardcode

                await _reservationService.CreateReservationAsync(userId, bookId);
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
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                // TODO: Lấy approvedBy từ session/claims
                int approvedBy = 2; // Thủ thư

                await _reservationService.ApproveReservationAsync(id, approvedBy);
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
        public async Task<IActionResult> Reject(int id, string reason)
        {
            try
            {
                // TODO: Lấy rejectedBy từ session/claims
                int rejectedBy = 2; // Thủ thư

                await _reservationService.RejectReservationAsync(id, rejectedBy, reason);
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

