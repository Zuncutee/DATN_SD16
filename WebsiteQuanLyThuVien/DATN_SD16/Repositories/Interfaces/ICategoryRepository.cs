using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface cho Category
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithBooksAsync(int categoryId);
    }
}

