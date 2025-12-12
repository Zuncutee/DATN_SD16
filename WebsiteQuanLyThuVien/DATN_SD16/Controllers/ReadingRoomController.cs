using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Attributes;
using DATN_SD16.Helpers;
using DATN_SD16.Models.Entities;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller cho Độc giả - Đặt phòng đọc
    /// </summary>
    [Authorize]
    [AuthorizeRoles("Reader")]
    public class ReadingRoomController : Controller
    {
        private readonly IReadingRoomService _readingRoomService;

        public ReadingRoomController(IReadingRoomService readingRoomService)
        {
            _readingRoomService = readingRoomService;
        }

        // GET: ReadingRoom
        public async Task<IActionResult> Index()
        {
            var rooms = await _readingRoomService.GetActiveRoomsAsync();
            return View(rooms);
        }

        // GET: ReadingRoom/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var room = await _readingRoomService.GetRoomWithSeatsAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            var availableSeats = await _readingRoomService.GetAvailableSeatsAsync(id);
            ViewBag.AvailableSeats = availableSeats.Count();
            
            return View(room);
        }

        // GET: ReadingRoom/Reserve/5
        public async Task<IActionResult> Reserve(int seatId)
        {
            var seat = await _readingRoomService.GetSeatByIdAsync(seatId);
            if (seat == null)
            {
                return NotFound();
            }

            ViewBag.Seat = seat;
            return View();
        }

        // POST: ReadingRoom/Reserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int seatId, DateTime reservationDate, int durationHours)
        {
            try
            {
                var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var startTime = reservationDate.TimeOfDay;
                var endTime = startTime.Add(TimeSpan.FromHours(durationHours));

                await _readingRoomService.CreateReservationAsync(userId, seatId, reservationDate, startTime, endTime);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Đặt chỗ thành công!" });
                }

                TempData["Success"] = "Đặt chỗ thành công!";
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Reserve), new { seatId });
            }
        }

        // GET: ReadingRoom/MyReservations
        public async Task<IActionResult> MyReservations()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var reservations = await _readingRoomService.GetReservationsByUserIdAsync(userId);
            return View(reservations);
        }

        // POST: ReadingRoom/CheckIn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id, string qrCode)
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
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyReservations));
            }
        }

        // POST: ReadingRoom/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
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
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyReservations));
            }
        }

        // POST: ReadingRoom/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
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
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyReservations));
            }
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   Request.Headers["Accept"].ToString().Contains("application/json");
        }
    }
}

