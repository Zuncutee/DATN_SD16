using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Models.DTOs;
using DATN_SD16.Helpers;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Models.Entities;

namespace DATN_SD16.Controllers
{
    // Controller đăng nhập Admin
    [AllowAnonymous]
    public class AdminAuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IRepository<PasswordResetToken> _passwordResetTokenRepository;

        public AdminAuthController(
            IAuthService authService, 
            IUserService userService,
            IEmailService emailService,
            IRepository<PasswordResetToken> passwordResetTokenRepository)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
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

        // POST: AdminAuth/ForgotPassword
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try 
            {
                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { success = false, message = "Vui lòng nhập email." });
                }

                // Kiểm tra email tồn tại
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null) 
                { 
                    // Để bảo mật, không báo lỗi nếu email không tồn tại
                    return Json(new { success = true, message = "Nếu email tồn tại, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu." }); 
                }

                // Generate Token
                var token = Guid.NewGuid().ToString("N");
                var resetToken = new PasswordResetToken
                {
                    UserId = user.UserId,
                    Token = token,
                    ExpiresAt = DateTime.Now.AddMinutes(15),
                    IsUsed = false,
                    CreatedAt = DateTime.Now
                };

                await _passwordResetTokenRepository.AddAsync(resetToken);

                // Send Email
                var resetLink = Url.Action("ResetPassword", "AdminAuth", new { token = token }, Request.Scheme);
                var subject = "Đặt lại mật khẩu - Thư viện";
                var body = $@"
                    <h3>Yêu cầu đặt lại mật khẩu</h3>
                    <p>Xin chào {user.FullName},</p>
                    <p>Bạn (hoặc ai đó) đã yêu cầu đặt lại mật khẩu cho tài khoản {user.Username}.</p>
                    <p>Vui lòng click vào link dưới đây để đặt lại mật khẩu:</p>
                    <p><a href='{resetLink}'>Đặt lại mật khẩu</a></p>
                    <p>Link này sẽ hết hạn sau 15 phút.</p>
                    <p>Nếu bạn không yêu cầu, vui lòng bỏ qua email này.</p>";

                await _emailService.SendEmailAsync(email, subject, body, "PasswordReset");

                return Json(new { success = true, message = "Nếu email tồn tại, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // GET: AdminAuth/ResetPassword
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var resetToken = await _passwordResetTokenRepository.FirstOrDefaultAsync(t => t.Token == token);
            if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.Now)
            {
                ViewBag.Error = "Link đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.";
                return View("Login"); // Fallback to login with error
            }

            var model = new ResetPasswordRequest { Token = token };
            return View(model);
        }

        // POST: AdminAuth/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            // Ignore IsValid false if only ConfirmPassword mismatch inside loop? No, default binding.
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var resetToken = await _passwordResetTokenRepository.FirstOrDefaultAsync(t => t.Token == model.Token);
                if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.Now)
                {
                    ModelState.AddModelError("", "Link đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
                    return View(model);
                }

                var user = await _userService.GetUserByIdAsync(resetToken.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "Người dùng không tồn tại.");
                    return View(model);
                }

                // Update Password
                var newPasswordHash = await _userService.HashPasswordAsync(model.NewPassword);
                user.PasswordHash = newPasswordHash;
                await _userService.UpdateUserAsync(user);

                // Mark token as used
                resetToken.IsUsed = true;
                await _passwordResetTokenRepository.UpdateAsync(resetToken);

                // Redirect to Login with success
                // Cannot pass complex object easily, use TempData or ViewBag in Redirect?
                // TempData is better.
                // Assuming TempData is available.
                // Or return View("Login") with ViewBag.Success
                ViewBag.Success = "Đổi mật khẩu thành công. Vui lòng đăng nhập.";
                return View("Login"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
                return View(model);
            }
        }
    }
}



