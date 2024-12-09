using Innoverse.Models.Domain;

namespace Innoverse.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file,BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAll();
    }
}
