using Innoverse.Models.Domain;
using Innoverse.Models.DTO;
using Innoverse.Repositories.Implementation;
using Innoverse.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Innoverse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {

        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;


        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto()
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    IsVisible = blogPost.IsVisible,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBlogPostById(Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if(blogPost == null)
            {
                return NotFound();
            }

            var response= new BlogPostDto()
            {
                Id = blogPost.Id,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    IsVisible = blogPost.IsVisible,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto requestDto)
        {
            var blogPost = new BlogPost()
            {
                Author = requestDto.Author,
                Content = requestDto.Content,
                PublishedDate = requestDto.PublishedDate,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                ShortDescription = requestDto.ShortDescription,
                Title = requestDto.Title,
                IsVisible = requestDto.IsVisible,
                UrlHandle = requestDto.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }


            blogPost = await _blogPostRepository.CreateAsync(blogPost);


            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };


            return Ok(response);

        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost()
            {
                Id=id,
                Author = request.Author,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                FeaturedImageUrl= request.FeaturedImageUrl,
                ShortDescription= request.ShortDescription,
                Title = request.Title,
                IsVisible = request.IsVisible,
                Categories=new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await _blogPostRepository.UpdateAsync(blogPost);

            if (blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };


            return Ok(response);

        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var blogPost = await _blogPostRepository.DeleteAsync(id);

            if(blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto()
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl
            };

            return Ok(response);

        }


       
    }
}
