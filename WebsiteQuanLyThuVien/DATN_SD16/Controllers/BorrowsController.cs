using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Attributes;
using DATN_SD16.Helpers;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý mượn-trả sách
    /// </summary>
    [Authorize]
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
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var borrows = await _borrowService.GetBorrowsByUserIdAsync(userId.Value);
            return View(borrows);
        }

        // GET: Borrows/MyBorrows
        public async Task<IActionResult> MyBorrows()
        {
            var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
            var activeBorrows = await _borrowService.GetActiveBorrowsByUserIdAsync(userId.Value);
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
        [AuthorizeRoles("Librarian", "Admin")]
        public async Task<IActionResult> Create(int copyId, int? reservationId)
        {
            try
            {
                var userId = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                var borrowedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");

                var borrow = await _borrowService.CreateBorrowAsync(userId.Value, copyId, borrowedBy.Value, reservationId);
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
        [AuthorizeRoles("Librarian", "Admin")]
        public async Task<IActionResult> Return(int id, string? conditionOnReturn)
        {
            try
            {
                var returnedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _borrowService.ReturnBookAsync(id, returnedBy.Value, conditionOnReturn);
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

