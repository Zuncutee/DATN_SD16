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
    [Authorize]
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
        public async Task<IActionResult> Index(string? title, string? author, int? categoryId, bool? availableOnly)
        {
            var books = await _bookService.SearchBooksAsync(title, author, categoryId, availableOnly);
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Title = title;
            ViewBag.Author = author;
            ViewBag.CategoryId = categoryId;
            ViewBag.AvailableOnly = availableOnly;
            return View(books);
        }

        // GET: Books/Details/5
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
    }
}

