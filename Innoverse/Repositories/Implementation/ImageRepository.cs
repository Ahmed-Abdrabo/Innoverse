using Innoverse.Data;
using Innoverse.Models.Domain;
using Innoverse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Innoverse.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        public ImageRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
           return await _context.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = _contextAccessor.HttpContext.Request;

            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await _context.BlogImages.AddAsync(blogImage);
            await _context.SaveChangesAsync();
            return blogImage;
        }   
    }
}
