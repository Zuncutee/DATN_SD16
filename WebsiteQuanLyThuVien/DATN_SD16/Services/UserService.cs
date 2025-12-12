using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho User
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserService(
            IUserRepository userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole> userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetUserWithRolesAsync(int userId)
        {
            return await _userRepository.GetUserWithRolesAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _userRepository.GetUsersByRoleAsync(roleName);
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.PasswordHash = await HashPasswordAsync(password);
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId, int assignedBy)
        {
            var existingRole = await _userRoleRepository.FirstOrDefaultAsync(
                ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (existingRole != null) return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.Now
            };

            await _userRoleRepository.AddAsync(userRole);
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(
                ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (userRole == null) return false;

            await _userRoleRepository.DeleteAsync(userRole);
            return true;
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _userRepository.IsUsernameExistsAsync(username);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _userRepository.IsEmailExistsAsync(email);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string passwordHash)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            var pass = "";
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordHash));
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            return await Task.FromResult(hashedPassword);
        }
    }
}

