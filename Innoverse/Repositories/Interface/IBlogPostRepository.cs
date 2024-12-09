using Innoverse.Models.Domain;

namespace Innoverse.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    }
}
