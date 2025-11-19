using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Models.DTOs;
using DATN_SD16.Helpers;

namespace DATN_SD16.Controllers
{
    /// <summary>
    /// Controller đăng nhập Admin
    /// </summary>
    [AllowAnonymous]
    public class AdminAuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AdminAuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // GET: AdminAuth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: AdminAuth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var response = await _authService.LoginAsync(request);
                if (response == null)
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return View(request);
                }

                // Kiểm tra role Admin
                if (!response.UserInfo.Roles.Contains("Admin"))
                {
                    ViewBag.Error = "Bạn không có quyền truy cập trang quản trị";
                    return View(request);
                }

                // Lưu token vào cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps, // Chỉ secure trong HTTPS
                    SameSite = SameSiteMode.Lax, // Cho phép cross-site trong một số trường hợp
                    Expires = response.ExpiresAt
                };

                Response.Cookies.Append("AuthToken", response.Token, cookieOptions);

                var refreshCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(7) // Refresh token 7 ngày
                };

                Response.Cookies.Append("RefreshToken", response.RefreshToken, refreshCookieOptions);

                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(request);
            }
        }

        // POST: AdminAuth/Logout
        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("RefreshToken");
            return RedirectToAction("Login", "AdminAuth");
        }
    }
}

