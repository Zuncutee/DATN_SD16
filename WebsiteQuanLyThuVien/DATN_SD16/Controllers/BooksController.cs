using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Attributes;
using DATN_SD16.Helpers;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý sách
    /// </summary>
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryRepository _categoryRepository;

        public BooksController(IBookService bookService, ICategoryRepository categoryRepository)
        {
            _bookService = bookService;
            _categoryRepository = categoryRepository;
        }

        // GET: Books
        [HttpGet]
        [Route("Books")]
        [Route("Books/Index")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? title, string? author, string? categoryId, bool? availableOnly)
        {
            // Parse categoryId - handle empty string as null
            int? parsedCategoryId = null;
            if (!string.IsNullOrWhiteSpace(categoryId) && int.TryParse(categoryId, out int catId))
            {
                parsedCategoryId = catId;
            }

            var books = await _bookService.SearchBooksAsync(title, author, parsedCategoryId, availableOnly);
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Title = title;
            ViewBag.Author = author;
            ViewBag.CategoryId = parsedCategoryId;
            ViewBag.AvailableOnly = availableOnly;
            return View(books);
        }

        // GET: Books/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookWithDetailsAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [AuthorizeRoles("Admin", "Librarian")]
        public IActionResult Create()
        {
            return RedirectToAction("CreateBook", "Admin");
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public IActionResult Create(Models.Entities.Book book)
        {
            return RedirectToAction("CreateBook", "Admin");
        }

        // GET: Books/Edit/5
        [AuthorizeRoles("Admin", "Librarian")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return RedirectToAction("EditBook", "Admin", new { id = id.Value });
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public IActionResult Edit(int id, Models.Entities.Book book)
        {
            return RedirectToAction("EditBook", "Admin", new { id });
        }

        // GET: Books/Delete/5
        [AuthorizeRoles("Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return RedirectToAction("Books", "Admin");
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public IActionResult DeleteConfirmed(int id) => RedirectToAction("Books", "Admin");

        // GET: Books/GetAvailableCopies
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvailableCopies(int bookId)
        {
            try
            {
                if (bookId <= 0)
                {
                    return Json(new { error = "BookId không hợp lệ" });
                }

                var copies = await _bookService.GetAvailableCopiesAsync(bookId);
                
                var result = copies.Select(c => new
                {
                    copyId = c.CopyId,
                    copyNumber = c.CopyNumber ?? $"Copy-{c.CopyId}",
                    condition = c.Condition ?? "Good",
                    status = c.Status ?? "Unknown"
                }).ToList();

                // Log để debug
                System.Diagnostics.Debug.WriteLine($"GetAvailableCopies - BookId: {bookId}, Returned {result.Count} copies");

                return Json(result);
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Error in GetAvailableCopies: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { error = ex.Message });
            }
        }
    }
}

