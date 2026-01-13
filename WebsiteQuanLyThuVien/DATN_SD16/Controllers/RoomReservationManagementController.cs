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
    [Authorize]
    [AuthorizeRoles("Admin", "Librarian")]
    public class RoomReservationManagementController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IReadingRoomService _readingRoomService;
        private readonly IRepository<ReadingRoom> _readingRoomRepository;

        public RoomReservationManagementController(
            LibraryDbContext context,
            IReadingRoomService readingRoomService,
            IRepository<ReadingRoom> readingRoomRepository)
        {
            _context = context;
            _readingRoomService = readingRoomService;
            _readingRoomRepository = readingRoomRepository;
        }

        // GET: RoomReservationManagement/RoomReservations
        public async Task<IActionResult> RoomReservations(int? roomId, string? status, DateTime? date)
        {
            var query = _context.ReadingRoomReservations
                .Include(r => r.User)
                .Include(r => r.Seat)
                    .ThenInclude(s => s.Room)
                .OrderByDescending(r => r.CreatedAt)
                .AsQueryable();

            if (roomId.HasValue)
            {
                query = query.Where(r => r.Seat.RoomId == roomId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status);
            }

            if (date.HasValue)
            {
                query = query.Where(r => r.ReservationDate.Date == date.Value.Date);
            }

            var reservations = await query.ToListAsync();

            ViewBag.Rooms = await _readingRoomRepository.GetAllAsync();
            ViewBag.SelectedRoomId = roomId;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedDate = date;
            ViewBag.StatusList = new[] { "Reserved", "CheckedIn", "Completed", "Cancelled", "NoShow" };

            // Explicitly point to the view in Admin folder
            return View("~/Views/Admin/RoomReservations.cshtml", reservations);
        }

        // GET: RoomReservationManagement/RoomReservationDetails/5
        public async Task<IActionResult> RoomReservationDetails(int id)
        {
            var reservation = await _context.ReadingRoomReservations
                .Include(r => r.User)
                .Include(r => r.Seat)
                    .ThenInclude(s => s.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
            
            if (reservation == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/RoomReservationDetails.cshtml", reservation);
        }

        // POST: RoomReservationManagement/CheckInReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInReservation(int id, string qrCode)
        {
            try
            {
                var success = await _readingRoomService.CheckInAsync(id, qrCode);
                if (!success)
                {
                    throw new Exception("Check-in thất bại. Vui lòng kiểm tra lại mã QR.");
                }

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Check-in thành công!" });
                }

                TempData["Success"] = "Check-in thành công!";
                return RedirectToAction(nameof(RoomReservationDetails), new { id });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomReservationDetails), new { id });
            }
        }

        // POST: RoomReservationManagement/CheckOutReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutReservation(int id)
        {
            try
            {
                var success = await _readingRoomService.CheckOutAsync(id);
                if (!success)
                {
                    throw new Exception("Check-out thất bại.");
                }

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Check-out thành công!" });
                }

                TempData["Success"] = "Check-out thành công!";
                return RedirectToAction(nameof(RoomReservationDetails), new { id });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomReservationDetails), new { id });
            }
        }

        // POST: RoomReservationManagement/CancelRoomReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRoomReservation(int id)
        {
            try
            {
                var success = await _readingRoomService.CancelReservationAsync(id);
                if (!success)
                {
                    throw new Exception("Hủy đặt chỗ thất bại.");
                }

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Hủy đặt chỗ thành công!" });
                }

                TempData["Success"] = "Hủy đặt chỗ thành công!";
                return RedirectToAction(nameof(RoomReservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomReservations));
            }
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   Request.Headers["Accept"].ToString().Contains("application/json");
        }
    }
}
