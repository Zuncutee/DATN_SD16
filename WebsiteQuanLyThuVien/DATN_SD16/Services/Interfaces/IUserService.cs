using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Service interface cho User
    /// </summary>
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserWithRolesAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, int roleId, int assignedBy);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> ValidatePasswordAsync(string password, string passwordHash);
        Task<string> HashPasswordAsync(string password);
    }
}

