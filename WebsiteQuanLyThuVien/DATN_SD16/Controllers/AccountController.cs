using Microsoft.AspNetCore.Mvc;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Models.Entities;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller quản lý tài khoản (Đăng nhập, đăng ký)
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                return View();
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại hoặc đã bị khóa");
                return View();
            }

            var isValidPassword = await _userService.ValidatePasswordAsync(password, user.PasswordHash);
            if (!isValidPassword)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng");
                return View();
            }

            // TODO: Tạo session/claims và redirect
            // Tạm thời redirect về Home
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp");
                return View(user);
            }

            if (await _userService.IsUsernameExistsAsync(user.Username))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                return View(user);
            }

            if (await _userService.IsEmailExistsAsync(user.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
                return View(user);
            }

            try
            {
                var newUser = await _userService.CreateUserAsync(user, password);
                // TODO: Gán role Reader mặc định
                // await _userService.AssignRoleAsync(newUser.UserId, 3, 1); // Role Reader = 3
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            // TODO: Xóa session/claims
            return RedirectToAction("Index", "Home");
        }
    }
}

