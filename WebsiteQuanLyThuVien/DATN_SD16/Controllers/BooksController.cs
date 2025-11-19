using Microsoft.AspNetCore.Mvc;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;

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
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Entities.Book book)
        {
            if (ModelState.IsValid)
            {
                // TODO: Lấy userId từ session/claims
                int createdBy = 1; // Tạm thời hardcode
                await _bookService.CreateBookAsync(book, createdBy);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookByIdAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Entities.Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bookService.UpdateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

