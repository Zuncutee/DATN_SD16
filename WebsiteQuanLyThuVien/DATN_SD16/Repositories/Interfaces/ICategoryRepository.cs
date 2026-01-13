using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    // Repository interface cho Category
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithBooksAsync(int categoryId);
    }
}

