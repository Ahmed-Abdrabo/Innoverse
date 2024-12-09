using Innoverse.Data;
using Innoverse.Models.Domain;
using Innoverse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Innoverse.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory=await _context.Categories.FirstOrDefaultAsync(c => c.Id==category.Id);
            if (existingCategory != null)
            { 
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory is null)
            {
                return null;
            }
            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
    }
}
