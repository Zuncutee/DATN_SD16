using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface cho User
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithRolesAsync(int userId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
    }
}

