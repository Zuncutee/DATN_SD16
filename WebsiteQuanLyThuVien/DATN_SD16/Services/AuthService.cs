using DATN_SD16.Models.DTOs;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Authentication
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserService userService,
            IJwtService jwtService,
            IRepository<RefreshToken> refreshTokenRepository,
            IConfiguration configuration)
        {
            _userService = userService;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null || !user.IsActive)
                return null;

            var isValidPassword = await _userService.ValidatePasswordAsync(request.Password, user.PasswordHash);
            if (!isValidPassword)
                return null;

            var userWithRoles = await _userService.GetUserWithRolesAsync(user.UserId);
            var roles = userWithRoles?.UserRoles.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();

            var token = _jwtService.GenerateToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            user.LastLoginAt = DateTime.Now;
            await _userService.UpdateUserAsync(user);

            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
                UserInfo = new UserInfo
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles
                }
            };
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            if (await _userService.IsUsernameExistsAsync(request.Username))
                throw new Exception("Tên đăng nhập đã tồn tại");

            if (await _userService.IsEmailExistsAsync(request.Email))
                throw new Exception("Email đã tồn tại");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var newUser = await _userService.CreateUserAsync(user, request.Password);

            await _userService.AssignRoleAsync(newUser.UserId, 3, 1);

            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            return await LoginAsync(loginRequest);
        }

        public async Task<AuthResponse?> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return null;

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return null;

            var storedRefreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(
                rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

            if (storedRefreshToken == null)
                return null;

            var user = await _userService.GetUserWithRolesAsync(userId);
            if (user == null || !user.IsActive)
                return null;

            var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

            var newToken = _jwtService.GenerateToken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            return new AuthResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
                UserInfo = new UserInfo
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles
                }
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var storedRefreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(
                rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedRefreshToken == null)
                return false;

            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            return true;
        }
    }
}

