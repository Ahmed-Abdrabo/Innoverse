using Innoverse.Data;
using Innoverse.Models.Domain;
using Innoverse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Innoverse.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly ApplicationDbContext _context;

        public BlogPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _context.BlogPosts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingBlogPost is null)
            {
                return null;
            }
            _context.BlogPosts.Remove(existingBlogPost);
            await _context.SaveChangesAsync();
            return existingBlogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _context.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost= await _context.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id==blogPost.Id);
            if (existingBlogPost is null)
            {
                return null;
            }

            _context.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            existingBlogPost.Categories = blogPost.Categories;
            await _context.SaveChangesAsync();
            return blogPost;
        }
    }
}
