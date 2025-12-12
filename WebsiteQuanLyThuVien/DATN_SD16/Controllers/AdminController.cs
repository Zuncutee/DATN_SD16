using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Models.Entities;
using DATN_SD16.Helpers;
using DATN_SD16.Attributes;
using DATN_SD16.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

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
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly IBorrowService _borrowService;
        private readonly IReadingRoomService _readingRoomService;
        private readonly IRepository<ReadingRoom> _readingRoomRepository;
        private readonly IRepository<ReadingRoomSeat> _seatRepository;
        private readonly IRepository<ReadingRoomReservation> _roomReservationRepository;
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

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
            IBookRepository bookRepository,
            IRepository<BookCopy> bookCopyRepository,
            IBorrowService borrowService,
            IReadingRoomService readingRoomService,
            IRepository<ReadingRoom> readingRoomRepository,
            IRepository<ReadingRoomSeat> seatRepository,
            IRepository<ReadingRoomReservation> roomReservationRepository,
            LibraryDbContext context,
            IWebHostEnvironment webHostEnvironment)
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
            _bookCopyRepository = bookCopyRepository;
            _borrowService = borrowService;
            _readingRoomService = readingRoomService;
            _readingRoomRepository = readingRoomRepository;
            _seatRepository = seatRepository;
            _roomReservationRepository = roomReservationRepository;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<IActionResult> BorrowBook()
        {
            await PrepareBorrowBookDropdownsAsync();
            return View();
        }

        // POST: Admin/BorrowBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int userId, int copyId, int? reservationId)
        {
            try
            {
                var borrowedBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _borrowService.CreateBorrowAsync(userId, copyId, borrowedBy, reservationId);

                if (IsAjaxRequest())
                {
                    return Json(new
                    {
                        success = true,
                        message = "Tạo phiếu mượn thành công!",
                        redirectUrl = Url.Action(nameof(BorrowBook))
                    });
                }

                TempData["Success"] = "Tạo phiếu mượn thành công!";
                return RedirectToAction(nameof(BorrowBook));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                await PrepareBorrowBookDropdownsAsync();
                return View();
            }
        }

        #region Quản lý bản sách (BookCopies)
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> BookCopies(int? bookId)
        {
            var books = (await _bookService.GetAllBooksAsync()).OrderBy(b => b.Title).ToList();
            var selectedBookId = bookId ?? books.FirstOrDefault()?.BookId;
            IEnumerable<BookCopy> copies = Enumerable.Empty<BookCopy>();

            if (selectedBookId.HasValue)
            {
                copies = await _bookCopyRepository.FindAsync(c => c.BookId == selectedBookId.Value);
                copies = copies.OrderBy(c => c.CopyNumber);
            }

            ViewBag.Books = books;
            ViewBag.SelectedBookId = selectedBookId;
            return View(copies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> CreateBookCopy(BookCopy copy)
        {
            try
            {
                if (copy.BookId <= 0)
                {
                    throw new Exception("Vui lòng chọn sách.");
                }

                if (string.IsNullOrWhiteSpace(copy.CopyNumber))
                {
                    throw new Exception("Vui lòng nhập mã bản sách (CopyNumber).");
                }

                copy.Status = string.IsNullOrWhiteSpace(copy.Status) ? "Available" : copy.Status.Trim();
                copy.Condition = string.IsNullOrWhiteSpace(copy.Condition) ? "Good" : copy.Condition.Trim();
                copy.CreatedAt = DateTime.Now;
                copy.UpdatedAt = DateTime.Now;

                await _bookCopyRepository.AddAsync(copy);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Thêm bản sách thành công!" });
                }

                TempData["Success"] = "Thêm bản sách thành công!";
                return RedirectToAction(nameof(BookCopies), new { bookId = copy.BookId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(BookCopies), new { bookId = copy.BookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> UpdateBookCopyStatus(int copyId, string status, string? condition)
        {
            try
            {
                var copy = await _bookCopyRepository.GetByIdAsync(copyId);
                if (copy == null)
                {
                    throw new Exception("Không tìm thấy bản sách.");
                }

                if (string.IsNullOrWhiteSpace(status))
                {
                    throw new Exception("Vui lòng chọn trạng thái.");
                }

                copy.Status = status;

                if (!string.IsNullOrWhiteSpace(condition))
                {
                    copy.Condition = condition;
                }

                copy.UpdatedAt = DateTime.Now;
                await _bookCopyRepository.UpdateAsync(copy);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Cập nhật bản sách thành công!" });
                }

                TempData["Success"] = "Cập nhật bản sách thành công!";
                return RedirectToAction(nameof(BookCopies), new { bookId = copy.BookId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(BookCopies));
            }
        }
        #endregion

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
                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    return Json(new { success = false, message = "Tên đăng nhập không được để trống" });
                }

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return Json(new { success = false, message = "Email không được để trống" });
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    return Json(new { success = false, message = "Mật khẩu không được để trống" });
                }

                if (password.Length < 6)
                {
                    return Json(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });
                }

                if (await _userService.IsUsernameExistsAsync(user.Username))
                {
                    return Json(new { success = false, message = "Tên đăng nhập đã tồn tại" });
                }

                if (await _userService.IsEmailExistsAsync(user.Email))
                {
                    return Json(new { success = false, message = "Email đã tồn tại" });
                }

                if (roleIds == null || !roleIds.Any())
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một vai trò" });
                }

                var newUser = await _userService.CreateUserAsync(user, password);
                
                if (roleIds != null && roleIds.Any())
                {
                    var currentUserId = UserHelper.GetUserId(User) ?? 1;
                    foreach (var roleId in roleIds)
                    {
                        await _userService.AssignRoleAsync(newUser.UserId, roleId, currentUserId);
                    }
                }

                return Json(new { success = true, message = "Tạo tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
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
                
                var currentUser = await _userService.GetUserWithRolesAsync(user.UserId);
                var currentUserId = UserHelper.GetUserId(User) ?? 1;
                
                var rolesToRemove = currentUser?.UserRoles?.ToList() ?? new List<UserRole>();
                foreach (var userRole in rolesToRemove)
                {
                    await _userService.RemoveRoleAsync(user.UserId, userRole.RoleId);
                }
                
                if (roleIds != null && roleIds.Any())
                {
                    foreach (var roleId in roleIds)
                    {
                        await _userService.AssignRoleAsync(user.UserId, roleId, currentUserId);
                    }
                }

                return Json(new { success = true, message = "Cập nhật tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
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
                // Validation
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    return Json(new { success = false, message = "Tên thể loại không được để trống" });
                }

                var exists = await _categoryRepository.FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName);
                if (exists != null)
                {
                    return Json(new { success = false, message = "Tên thể loại đã tồn tại" });
                }

                category.CreatedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;

                await _categoryRepository.AddAsync(category);
                return Json(new { success = true, message = "Thêm thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: Admin/Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    return Json(new { success = false, message = "Tên thể loại không được để trống" });
                }

                var existing = await _categoryRepository.GetByIdAsync(category.CategoryId);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thể loại" });
                }

                var duplicate = await _categoryRepository.FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);
                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Tên thể loại đã tồn tại" });
                }

                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;
                existing.UpdatedAt = DateTime.Now;

                await _categoryRepository.UpdateAsync(existing);
                return Json(new { success = true, message = "Cập nhật thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thể loại" });
                }

                var booksWithCategory = await _bookRepository.FindAsync(b => b.CategoryId == id);
                if (booksWithCategory.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa thể loại này vì đang có sách sử dụng" });
                }

                await _categoryRepository.DeleteAsync(category);
                return Json(new { success = true, message = "Xóa thể loại thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // GET: Admin/Authors
        public async Task<IActionResult> Authors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return View(authors);
        }

        // POST: Admin/Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(author.AuthorName))
                {
                    return Json(new { success = false, message = "Tên tác giả không được để trống" });
                }

                await _authorRepository.AddAsync(author);
                return Json(new { success = true, message = "Thêm tác giả thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Authors/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthor(Author author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(author.AuthorName))
                {
                    return Json(new { success = false, message = "Tên tác giả không được để trống" });
                }

                var existing = await _authorRepository.GetByIdAsync(author.AuthorId);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tác giả" });
                }

                existing.AuthorName = author.AuthorName;
                existing.Nationality = author.Nationality;
                existing.Biography = author.Biography;
                existing.UpdatedAt = DateTime.Now;

                await _authorRepository.UpdateAsync(existing);
                return Json(new { success = true, message = "Cập nhật tác giả thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(id);
                if (author == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tác giả" });
                }

                var booksWithAuthor = await _bookRepository.FindAsync(b => b.BookAuthors.Any(ba => ba.AuthorId == id));
                if (booksWithAuthor.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa tác giả này vì đang có sách sử dụng" });
                }

                await _authorRepository.DeleteAsync(author);
                return Json(new { success = true, message = "Xóa tác giả thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/Publishers
        public async Task<IActionResult> Publishers()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return View(publishers);
        }

        // POST: Admin/Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePublisher(Publisher publisher)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(publisher.PublisherName))
                {
                    return Json(new { success = false, message = "Tên nhà xuất bản không được để trống" });
                }

                var exists = await _publisherRepository.FirstOrDefaultAsync(p => p.PublisherName == publisher.PublisherName);
                if (exists != null)
                {
                    return Json(new { success = false, message = "Tên nhà xuất bản đã tồn tại" });
                }

                await _publisherRepository.AddAsync(publisher);
                return Json(new { success = true, message = "Thêm nhà xuất bản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Publishers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublisher(Publisher publisher)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(publisher.PublisherName))
                {
                    return Json(new { success = false, message = "Tên nhà xuất bản không được để trống" });
                }

                var existing = await _publisherRepository.GetByIdAsync(publisher.PublisherId);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà xuất bản" });
                }

                var duplicate = await _publisherRepository.FirstOrDefaultAsync(p => p.PublisherName == publisher.PublisherName && p.PublisherId != publisher.PublisherId);
                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Tên nhà xuất bản đã tồn tại" });
                }

                existing.PublisherName = publisher.PublisherName;
                existing.Address = publisher.Address;
                existing.PhoneNumber = publisher.PhoneNumber;
                existing.Email = publisher.Email;
                existing.UpdatedAt = DateTime.Now;

                await _publisherRepository.UpdateAsync(existing);
                return Json(new { success = true, message = "Cập nhật nhà xuất bản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Publishers/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var publisher = await _publisherRepository.GetByIdAsync(id);
                if (publisher == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà xuất bản" });
                }

                var booksWithPublisher = await _bookRepository.FindAsync(b => b.PublisherId == id);
                if (booksWithPublisher.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa nhà xuất bản này vì đang có sách sử dụng" });
                }

                await _publisherRepository.DeleteAsync(publisher);
                return Json(new { success = true, message = "Xóa nhà xuất bản thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/BookLocations
        public async Task<IActionResult> BookLocations()
        {
            var locations = await _bookLocationRepository.GetAllAsync();
            return View(locations);
        }

        // POST: Admin/BookLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBookLocation(BookLocation location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location.LocationCode))
                {
                    return Json(new { success = false, message = "Mã vị trí không được để trống" });
                }

                var exists = await _bookLocationRepository.FirstOrDefaultAsync(l => l.LocationCode == location.LocationCode);
                if (exists != null)
                {
                    return Json(new { success = false, message = "Mã vị trí đã tồn tại" });
                }

                await _bookLocationRepository.AddAsync(location);
                return Json(new { success = true, message = "Thêm vị trí sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/BookLocations/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookLocation(BookLocation location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location.LocationCode))
                {
                    return Json(new { success = false, message = "Mã vị trí không được để trống" });
                }

                var existing = await _bookLocationRepository.GetByIdAsync(location.LocationId);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy vị trí sách" });
                }

                var duplicate = await _bookLocationRepository.FirstOrDefaultAsync(l => l.LocationCode == location.LocationCode && l.LocationId != location.LocationId);
                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Mã vị trí đã tồn tại" });
                }

                existing.LocationCode = location.LocationCode;
                existing.ShelfNumber = location.ShelfNumber;
                existing.RowNumber = location.RowNumber;
                existing.Description = location.Description;
                existing.UpdatedAt = DateTime.Now;

                await _bookLocationRepository.UpdateAsync(existing);
                return Json(new { success = true, message = "Cập nhật vị trí sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/BookLocations/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteBookLocation(int id)
        {
            try
            {
                var location = await _bookLocationRepository.GetByIdAsync(id);
                if (location == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy vị trí sách" });
                }

                var booksUsingLocation = await _bookRepository.FindAsync(b => b.LocationId == id);
                if (booksUsingLocation.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa vị trí này vì đang có sách sử dụng" });
                }

                await _bookLocationRepository.DeleteAsync(location);
                return Json(new { success = true, message = "Xóa vị trí sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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

        // GET: Admin/Books/Create
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> CreateBook()
        {
            await PopulateBookDropdownsAsync();
            ViewBag.FormAction = nameof(CreateBook);
            ViewBag.PageTitle = "Thêm sách mới";
            ViewBag.IsEdit = false;
            return View("BookForm", new Book());
        }

        // POST: Admin/Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> CreateBook(Book book, IFormFile? coverImageFile)
        {
            try
            {
                if (book.CategoryId <= 0)
                {
                    ModelState.AddModelError(nameof(book.CategoryId), "Vui lòng chọn thể loại sách.");
                }

                if (!ModelState.IsValid)
                {
                    if (IsAjaxRequest())
                    {
                        return Json(new { success = false, message = BuildModelErrorMessage() });
                    }
                    await PopulateBookDropdownsAsync();
                    ViewBag.FormAction = nameof(CreateBook);
                    ViewBag.PageTitle = "Thêm sách mới";
                    ViewBag.IsEdit = false;
                    return View("BookForm", book);
                }

                book.CoverImage = await SaveCoverImageAsync(coverImageFile);
                var createdBy = UserHelper.GetUserId(User) ?? throw new UnauthorizedAccessException("User not authenticated");
                await _bookService.CreateBookAsync(book, createdBy);
                var message = "Thêm sách mới thành công!";
                TempData["Success"] = message;

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message, redirectUrl = Url.Action(nameof(Books)) });
                }

                return RedirectToAction(nameof(Books));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }

                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateBookDropdownsAsync();
                ViewBag.FormAction = nameof(CreateBook);
                ViewBag.PageTitle = "Thêm sách mới";
                ViewBag.IsEdit = false;
                return View("BookForm", book);
            }
        }

        // GET: Admin/Books/Edit/5
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await PopulateBookDropdownsAsync();
            ViewBag.FormAction = nameof(EditBook);
            ViewBag.PageTitle = "Chỉnh sửa sách";
            ViewBag.IsEdit = true;
            return View("BookForm", book);
        }

        // POST: Admin/Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> EditBook(Book book, IFormFile? coverImageFile)
        {
            try
            {
                if (book.BookId <= 0)
                {
                    var errorMsg = "Không tìm thấy ID sách cần cập nhật.";
                    if (IsAjaxRequest())
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    return NotFound();
                }

                // Validate CategoryId manually
                if (book.CategoryId <= 0)
                {
                    ModelState.AddModelError(nameof(book.CategoryId), "Vui lòng chọn thể loại sách.");
                }

                if (!ModelState.IsValid)
                {
                    if (IsAjaxRequest())
                    {
                        return Json(new { success = false, message = BuildModelErrorMessage() });
                    }
                    await PopulateBookDropdownsAsync();
                    ViewBag.FormAction = nameof(EditBook);
                    ViewBag.PageTitle = "Chỉnh sửa sách";
                    ViewBag.IsEdit = true;
                    return View("BookForm", book);
                }

                book.CoverImage = await SaveCoverImageAsync(coverImageFile, book.CoverImage);
                var success = await _bookService.UpdateBookAsync(book);
                if (!success)
                {
                    var errorMsg = "Không tìm thấy sách cần cập nhật.";
                    if (IsAjaxRequest())
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    return NotFound();
                }

                var message = "Cập nhật sách thành công!";
                TempData["Success"] = message;

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message, redirectUrl = Url.Action(nameof(Books)) });
                }

                return RedirectToAction(nameof(Books));
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                // You can add logging here if needed
                
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }

                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateBookDropdownsAsync();
                ViewBag.FormAction = nameof(EditBook);
                ViewBag.PageTitle = "Chỉnh sửa sách";
                ViewBag.IsEdit = true;
                return View("BookForm", book);
            }
        }

        // POST: Admin/Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success)
            {
                TempData["Error"] = "Không tìm thấy sách cần xóa.";
            }
            else
            {
                TempData["Success"] = "Đã xóa sách thành công.";
            }
            return RedirectToAction(nameof(Books));
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

        private async Task PopulateBookDropdownsAsync()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Publishers = await _publisherRepository.GetAllAsync();
            ViewBag.Locations = await _bookLocationRepository.GetAllAsync();
        }

        private async Task PrepareBorrowBookDropdownsAsync()
        {
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Books = await _bookService.GetAllBooksAsync();
        }

        private bool IsAjaxRequest()
        {
            if (Request?.Headers.TryGetValue("X-Requested-With", out var requestedWith) == true &&
                string.Equals(requestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (Request?.Headers.TryGetValue("Accept", out var acceptValues) == true)
            {
                return acceptValues.Any(value => value != null &&
                    value.Contains("application/json", StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        private string BuildModelErrorMessage()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => !string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.ErrorMessage : e.Exception?.Message)
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .ToList();

            return errors.Any()
                ? string.Join(" ", errors)
                : "Vui lòng kiểm tra lại các trường dữ liệu.";
        }

        private async Task<string?> SaveCoverImageAsync(IFormFile? coverImageFile, string? existingPath = null)
        {
            if (coverImageFile == null || coverImageFile.Length == 0)
            {
                return existingPath;
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "books");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(coverImageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await coverImageFile.CopyToAsync(stream);
            }

            if (!string.IsNullOrWhiteSpace(existingPath))
            {
                var oldPhysicalPath = GetPhysicalPath(existingPath);
                if (System.IO.File.Exists(oldPhysicalPath))
                {
                    System.IO.File.Delete(oldPhysicalPath);
                }
            }

            return $"/uploads/books/{fileName}";
        }

        private string GetPhysicalPath(string relativePath)
        {
            var cleanPath = relativePath.TrimStart('~').TrimStart('/');
            cleanPath = cleanPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            return Path.Combine(_webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), cleanPath);
        }

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

        #region Quản lý Phòng đọc
        // GET: Admin/ReadingRooms
        public async Task<IActionResult> ReadingRooms()
        {
            var rooms = await _readingRoomRepository.GetAllAsync();
            return View(rooms);
        }

        // POST: Admin/CreateReadingRoom
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReadingRoom(ReadingRoom room)
        {
            try
            {
                var createdBy = UserHelper.GetUserId(User) ?? 1;
                await _readingRoomService.CreateRoomAsync(room, createdBy);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo phòng đọc thành công!" });
                }

                TempData["Success"] = "Tạo phòng đọc thành công!";
                return RedirectToAction(nameof(ReadingRooms));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ReadingRooms));
            }
        }

        // POST: Admin/UpdateReadingRoom
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReadingRoom(ReadingRoom room)
        {
            try
            {
                await _readingRoomService.UpdateRoomAsync(room);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Cập nhật phòng đọc thành công!" });
                }

                TempData["Success"] = "Cập nhật phòng đọc thành công!";
                return RedirectToAction(nameof(ReadingRooms));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ReadingRooms));
            }
        }

        // POST: Admin/DeleteReadingRoom/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReadingRoom(int id)
        {
            try
            {
                await _readingRoomService.DeleteRoomAsync(id);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Xóa phòng đọc thành công!" });
                }

                TempData["Success"] = "Xóa phòng đọc thành công!";
                return RedirectToAction(nameof(ReadingRooms));
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(ReadingRooms));
            }
        }

        // GET: Admin/RoomSeats/5
        public async Task<IActionResult> RoomSeats(int id)
        {
            var room = await _readingRoomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            var seats = await _readingRoomService.GetSeatsByRoomIdAsync(id);
            
            ViewBag.Room = room;
            return View(seats);
        }

        // POST: Admin/CreateSeat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeat(ReadingRoomSeat seat)
        {
            try
            {
                await _readingRoomService.CreateSeatAsync(seat);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo chỗ ngồi thành công!" });
                }

                TempData["Success"] = "Tạo chỗ ngồi thành công!";
                return RedirectToAction(nameof(RoomSeats), new { id = seat.RoomId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomSeats), new { id = seat.RoomId });
            }
        }

        // POST: Admin/UpdateSeat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSeat(ReadingRoomSeat seat)
        {
            try
            {
                await _readingRoomService.UpdateSeatAsync(seat);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Cập nhật chỗ ngồi thành công!" });
                }

                TempData["Success"] = "Cập nhật chỗ ngồi thành công!";
                return RedirectToAction(nameof(RoomSeats), new { id = seat.RoomId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomSeats), new { id = seat.RoomId });
            }
        }

        // POST: Admin/DeleteSeat/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSeat(int id, int roomId)
        {
            try
            {
                await _readingRoomService.DeleteSeatAsync(id);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Xóa chỗ ngồi thành công!" });
                }

                TempData["Success"] = "Xóa chỗ ngồi thành công!";
                return RedirectToAction(nameof(RoomSeats), new { id = roomId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomSeats), new { id = roomId });
            }
        }

        // POST: Admin/GenerateQRCode/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateQRCode(int id, int roomId)
        {
            try
            {
                var seat = await _readingRoomService.GetSeatByIdAsync(id);
                if (seat == null)
                {
                    throw new Exception("Không tìm thấy chỗ ngồi");
                }

                // Generate new QR code
                var qrCode = await _readingRoomService.GenerateQRCodeForSeatAsync(id);
                
                // Update seat with new QR code
                seat.QRCode = qrCode;
                seat.UpdatedAt = DateTime.Now;
                await _readingRoomService.UpdateSeatAsync(seat);

                if (IsAjaxRequest())
                {
                    return Json(new { success = true, message = "Tạo mã QR thành công!", qrCode = qrCode });
                }

                TempData["Success"] = $"Tạo mã QR thành công: {qrCode}";
                return RedirectToAction(nameof(RoomSeats), new { id = roomId });
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(RoomSeats), new { id = roomId });
            }
        }

        // GET: Admin/RoomReservations
        [AuthorizeRoles("Admin", "Librarian")]
        public async Task<IActionResult> RoomReservations(int? roomId, string? status, DateTime? date)
        {
            // Get all reservations with navigation properties
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

            return View(reservations);
        }

        // GET: Admin/RoomReservationDetails/5
        [AuthorizeRoles("Admin", "Librarian")]
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

            return View(reservation);
        }

        // POST: Admin/CheckInReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
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

        // POST: Admin/CheckOutReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
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

        // POST: Admin/CancelRoomReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Admin", "Librarian")]
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
        #endregion
    }
}

