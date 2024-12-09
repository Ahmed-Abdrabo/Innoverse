using Innoverse.Models.Domain;

namespace Innoverse.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid Id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid Id);


    }
}
