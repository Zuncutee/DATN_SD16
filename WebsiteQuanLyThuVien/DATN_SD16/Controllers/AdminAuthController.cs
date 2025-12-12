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
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginRequest? jsonRequest, [FromForm] LoginRequest? formRequest)
        {
            bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            
            var request = jsonRequest ?? formRequest;
            
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                if (isAjaxRequest)
                {
                    var errorMsg = "Dữ liệu không hợp lệ";
                    if (request == null)
                        errorMsg = "Không nhận được dữ liệu đăng nhập. Vui lòng thử lại.";
                    else if (string.IsNullOrWhiteSpace(request.Username))
                        errorMsg = "Vui lòng nhập tên đăng nhập";
                    else if (string.IsNullOrWhiteSpace(request.Password))
                        errorMsg = "Vui lòng nhập mật khẩu";
                    
                    return Json(new { success = false, message = errorMsg });
                }
                return View(request);
            }

            try
            {
                var response = await _authService.LoginAsync(request);
                if (response == null)
                {
                    var errorMsg = "Tên đăng nhập hoặc mật khẩu không đúng";
                    if (isAjaxRequest)
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    ViewBag.Error = errorMsg;
                    return View(request);
                }

                var roles = response.UserInfo.Roles ?? new List<string>();

                string redirectController = "";
                string redirectAction = "";

                if (roles.Contains("Admin"))
                {
                    redirectController = "Admin";
                    redirectAction = "Dashboard";
                }
                else if (roles.Contains("Librarian"))
                {
                    redirectController = "Librarian";
                    redirectAction = "Dashboard";
                }
                else if (roles.Contains("Reader"))
                {
                    redirectController = "Reader";
                    redirectAction = "Dashboard";
                }
                else
                {
                    var errorMsg = "Tài khoản không có quyền truy cập hệ thống.";
                    if (isAjaxRequest)
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    ViewBag.Error = errorMsg;
                    return View(request);
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = response.ExpiresAt
                };

                Response.Cookies.Append("AuthToken", response.Token, cookieOptions);

                var refreshCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                Response.Cookies.Append("RefreshToken", response.RefreshToken, refreshCookieOptions);

                if (isAjaxRequest)
                {
                    return Json(new 
                    { 
                        success = true, 
                        message = "Đăng nhập thành công",
                        redirectUrl = Url.Action(redirectAction, redirectController)
                    });
                }

                // Điều hướng theo role
                return RedirectToAction(redirectAction, redirectController);
            }
            catch (Exception ex)
            {
                if (isAjaxRequest)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                ViewBag.Error = ex.Message;
                return View(request);
            }
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(request);
        //    }

        //    try
        //    {
        //        var response = await _authService.LoginAsync(request);
        //        if (response == null)
        //        {
        //            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
        //            return View(request);
        //        }

        //        // Kiểm tra role Admin
        //        if (!response.UserInfo.Roles.Contains("Admin"))
        //        {
        //            ViewBag.Error = "Bạn không có quyền truy cập trang quản trị";
        //            return View(request);
        //        }

        //        // Lưu token vào cookie
        //        var cookieOptions = new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Secure = Request.IsHttps, // Chỉ secure trong HTTPS
        //            SameSite = SameSiteMode.Lax, // Cho phép cross-site trong một số trường hợp
        //            Expires = response.ExpiresAt
        //        };

        //        Response.Cookies.Append("AuthToken", response.Token, cookieOptions);

        //        var refreshCookieOptions = new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Secure = Request.IsHttps,
        //            SameSite = SameSiteMode.Lax,
        //            Expires = DateTime.UtcNow.AddDays(7) // Refresh token 7 ngày
        //        };

        //        Response.Cookies.Append("RefreshToken", response.RefreshToken, refreshCookieOptions);

        //        return RedirectToAction("Dashboard", "Admin");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.Message;
        //        return View(request);
        //    }
        //}

        // POST: AdminAuth/Register
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                var existingUser = await _userService.GetUserByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Tên đăng nhập đã tồn tại" });
                }

                var emailExists = await _userService.IsEmailExistsAsync(request.Email);
                if (emailExists)
                {
                    return Json(new { success = false, message = "Email đã được sử dụng" });
                }

                var newUser = new Models.Entities.User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Address = request.Address,
                    IsActive = true,
                    IsLocked = false,
                    FailedLoginAttempts = 0
                };

                var createdUser = await _userService.CreateUserAsync(newUser, request.Password);

                await _userService.AssignRoleAsync(createdUser.UserId, 3, 1);

                return Json(new 
                { 
                    success = true, 
                    message = "Đăng ký thành công! Vui lòng đăng nhập." 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: AdminAuth/Logout
        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("RefreshToken");
            return RedirectToAction("Index", "Home");
        }
    }
}

// DTO for Register Request
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
}

