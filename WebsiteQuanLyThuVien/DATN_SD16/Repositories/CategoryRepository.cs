using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Repositories
{
    /// <summary>
    /// Repository implementation cho Category
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryWithBooksAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }
}

