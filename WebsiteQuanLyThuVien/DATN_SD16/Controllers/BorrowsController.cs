using Microsoft.AspNetCore.Mvc;
using DATN_SD16.Services.Interfaces;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý mượn-trả sách
    /// </summary>
    public class BorrowsController : Controller
    {
        private readonly IBorrowService _borrowService;

        public BorrowsController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            // TODO: Lấy userId từ session/claims
            int userId = 1; // Tạm thời hardcode
            var borrows = await _borrowService.GetBorrowsByUserIdAsync(userId);
            return View(borrows);
        }

        // GET: Borrows/MyBorrows
        public async Task<IActionResult> MyBorrows()
        {
            // TODO: Lấy userId từ session/claims
            int userId = 1; // Tạm thời hardcode
            var activeBorrows = await _borrowService.GetActiveBorrowsByUserIdAsync(userId);
            return View(activeBorrows);
        }

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(borrow);
        }

        // GET: Borrows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borrows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int copyId, int? reservationId)
        {
            try
            {
                // TODO: Lấy userId và borrowedBy từ session/claims
                int userId = 1;
                int borrowedBy = 2; // Thủ thư

                var borrow = await _borrowService.CreateBorrowAsync(userId, copyId, borrowedBy, reservationId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // POST: Borrows/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, string? conditionOnReturn)
        {
            try
            {
                // TODO: Lấy returnedBy từ session/claims
                int returnedBy = 2; // Thủ thư

                await _borrowService.ReturnBookAsync(id, returnedBy, conditionOnReturn);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // POST: Borrows/Renew/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(int id)
        {
            try
            {
                await _borrowService.RenewBorrowAsync(id);
                return RedirectToAction(nameof(MyBorrows));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}

