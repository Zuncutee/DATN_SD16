using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Models.Entities;
using DATN_SD16.Helpers;
using DATN_SD16.Attributes;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản trị dành cho Admin
    /// </summary>
    [Authorize]
    [AuthorizeRoles("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IRepository<BookLocation> _bookLocationRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;

        public AdminController(
            IUserService userService,
            IBookService bookService,
            IRepository<Category> categoryRepository,
            IRepository<Author> authorRepository,
            IRepository<Publisher> publisherRepository,
            IRepository<BookLocation> bookLocationRepository,
            IRepository<Role> roleRepository,
            IRepository<SystemSetting> systemSettingRepository,
            IBorrowRepository borrowRepository,
            IBookRepository bookRepository)
        {
            _userService = userService;
            _bookService = bookService;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _bookLocationRepository = bookLocationRepository;
            _roleRepository = roleRepository;
            _systemSettingRepository = systemSettingRepository;
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var allUsers = await _userService.GetAllUsersAsync();
            var allBooks = await _bookService.GetAllBooksAsync();
            var allBorrows = await _borrowRepository.GetAllAsync();
            var overdueBorrows = await _borrowRepository.GetOverdueBorrowsAsync();
            
            ViewBag.TotalUsers = allUsers.Count();
            ViewBag.TotalBooks = allBooks.Count();
            ViewBag.TotalBorrows = allBorrows.Count();
            ViewBag.OverdueBorrows = overdueBorrows.Count();
            return View();
        }

        #region Quản lý Tài khoản & Phân quyền
        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersWithRoles = new List<object>();
            
            foreach (var user in users)
            {
                var userWithRoles = await _userService.GetUserWithRolesAsync(user.UserId);
                usersWithRoles.Add(new { User = user, Roles = userWithRoles?.UserRoles.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>() });
            }
            
            ViewBag.UsersWithRoles = usersWithRoles;
            ViewBag.Roles = await _roleRepository.GetAllAsync();
            return View(users);
        }

        // GET: Admin/Users/Create
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles = await _roleRepository.GetAllAsync();
            return PartialView("_CreateUserModal");
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user, string password, List<int> roleIds)
        {
            try
            {
                var newUser = await _userService.CreateUserAsync(user, password);
                
                // Gán roles
                var currentUserId = UserHelper.GetUserId(User) ?? 1;
                foreach (var roleId in roleIds ?? new List<int>())
                {
                    await _userService.AssignRoleAsync(newUser.UserId, roleId, currentUserId);
                }

                return Json(new { success = true, message = "Tạo tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userService.GetUserWithRolesAsync(id);
            if (user == null) return NotFound();
            
            ViewBag.Roles = await _roleRepository.GetAllAsync();
            ViewBag.UserRoles = user.UserRoles.Select(ur => ur.RoleId).ToList();
            return PartialView("_EditUserModal", user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user, List<int> roleIds)
        {
            try
            {
                await _userService.UpdateUserAsync(user);
                
                // Cập nhật roles
                var currentUser = await _userService.GetUserWithRolesAsync(user.UserId);
                var currentUserId = UserHelper.GetUserId(User) ?? 1;
                
                // Xóa tất cả roles cũ
                foreach (var userRole in currentUser?.UserRoles ?? new List<UserRole>())
                {
                    await _userService.RemoveRoleAsync(user.UserId, userRole.RoleId);
                }
                
                // Thêm roles mới
                foreach (var roleId in roleIds ?? new List<int>())
                {
                    await _userService.AssignRoleAsync(user.UserId, roleId, currentUserId);
                }

                return Json(new { success = true, message = "Cập nhật tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Users/ToggleLock/5
        [HttpPost]
        public async Task<IActionResult> ToggleLock(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return Json(new { success = false, message = "Không tìm thấy người dùng" });

                user.IsLocked = !user.IsLocked;
                if (user.IsLocked)
                {
                    user.LockedUntil = DateTime.Now.AddDays(30); // Khóa 30 ngày
                }
                else
                {
                    user.LockedUntil = null;
                }

                await _userService.UpdateUserAsync(user);
                return Json(new { success = true, message = user.IsLocked ? "Đã khóa tài khoản" : "Đã mở khóa tài khoản" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Quản lý Danh mục
        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                return Json(new { success = true, message = "Thêm thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            try
            {
                await _categoryRepository.UpdateAsync(category);
                return Json(new { success = true, message = "Cập nhật thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null) return Json(new { success = false, message = "Không tìm thấy thể loại" });
                
                await _categoryRepository.DeleteAsync(category);
                return Json(new { success = true, message = "Xóa thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/Authors
        public async Task<IActionResult> Authors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return View(authors);
        }

        // GET: Admin/Publishers
        public async Task<IActionResult> Publishers()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return View(publishers);
        }

        // GET: Admin/BookLocations
        public async Task<IActionResult> BookLocations()
        {
            var locations = await _bookLocationRepository.GetAllAsync();
            return View(locations);
        }
        #endregion

        #region Quản lý Sách
        // GET: Admin/Books
        public async Task<IActionResult> Books()
        {
            var books = await _bookService.GetAllBooksAsync();
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Publishers = await _publisherRepository.GetAllAsync();
            ViewBag.Locations = await _bookLocationRepository.GetAllAsync();
            return View(books);
        }

        // POST: Admin/Books/Import
        [HttpPost]
        public async Task<IActionResult> ImportBooks(int bookId, int quantity)
        {
            try
            {
                var currentUserId = UserHelper.GetUserId(User) ?? 1;
                await _bookService.ImportBooksAsync(bookId, quantity, currentUserId);
                return Json(new { success = true, message = $"Đã nhập {quantity} bản sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Quản lý Hệ thống
        // GET: Admin/Settings
        public async Task<IActionResult> Settings()
        {
            var settings = await _systemSettingRepository.GetAllAsync();
            return View(settings);
        }

        // POST: Admin/Settings/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSetting(int settingId, string settingValue)
        {
            try
            {
                var setting = await _systemSettingRepository.GetByIdAsync(settingId);
                if (setting == null) return Json(new { success = false, message = "Không tìm thấy cấu hình" });

                setting.SettingValue = settingValue;
                setting.UpdatedAt = DateTime.Now;
                setting.UpdatedBy = UserHelper.GetUserId(User);
                await _systemSettingRepository.UpdateAsync(setting);

                return Json(new { success = true, message = "Cập nhật cấu hình thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Báo cáo & Thống kê
        // GET: Admin/Reports
        public async Task<IActionResult> Reports()
        {
            ViewBag.MostBorrowedBooks = (await _bookService.GetMostBorrowedBooksAsync(10)).ToList();
            ViewBag.OverdueBorrows = (await _borrowRepository.GetOverdueBorrowsAsync()).ToList();
            return View();
        }

        // GET: Admin/Reports/MostBorrowedBooks
        public async Task<IActionResult> MostBorrowedBooks()
        {
            var books = await _bookService.GetMostBorrowedBooksAsync(20);
            return PartialView("_MostBorrowedBooks", books);
        }

        // GET: Admin/Reports/OverdueBooks
        public async Task<IActionResult> OverdueBooks()
        {
            var borrows = await _borrowRepository.GetOverdueBorrowsAsync();
            return PartialView("_OverdueBooks", borrows);
        }
        #endregion
    }
}

