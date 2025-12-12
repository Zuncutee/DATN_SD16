using System.Diagnostics;
using DATN_SD16.Models;
using Microsoft.AspNetCore.Mvc;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;

namespace DATN_SD16.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IBookService bookService, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _bookService = bookService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Get latest books (last 8 books)
            var allBooks = await _bookService.GetAllBooksAsync();
            var latestBooks = allBooks.OrderByDescending(b => b.CreatedAt).Take(8).ToList();
            
            // Get popular categories (top 6)
            var categories = await _categoryRepository.GetAllAsync();
            var topCategories = categories.Take(6).ToList();
            
            // Get most borrowed books
            var popularBooks = await _bookService.GetMostBorrowedBooksAsync(8);
            
            ViewBag.LatestBooks = latestBooks;
            ViewBag.TopCategories = topCategories;
            ViewBag.PopularBooks = popularBooks;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
