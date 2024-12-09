using Innoverse.Models.Domain;
using Innoverse.Models.DTO;
using Innoverse.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Innoverse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();

            var response = new List<BlogImageDto>();
            foreach (var blogImage in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    Url = blogImage.Url,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                });
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName,
            [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };
                blogImage = await _imageRepository.Upload(file, blogImage);

                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    Url = blogImage.Url,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                };

                return Ok(response);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
