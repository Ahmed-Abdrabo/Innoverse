using Innoverse.Data;
using Innoverse.Models.Domain;
using Innoverse.Models.DTO;
using Innoverse.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innoverse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto() {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }
            return Ok(response);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var Category = await _categoryRepository.GetByIdAsync(id);

            if (Category == null)
            {
                return NotFound();
            }
            var response = new CategoryDto()
            {
                Id = Category.Id,
                Name = Category.Name,
                UrlHandle = Category.UrlHandle,
            };
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            var category = new Category()
            {
                Name = createCategoryRequestDto.Name,
                UrlHandle = createCategoryRequestDto.UrlHandle,
            };

            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestDto categoryDto, [FromRoute] Guid id)
        {
            var category = new Category()
            {
                Id = id,
                Name = categoryDto.Name,
                UrlHandle = categoryDto.UrlHandle,
            };

            category = await _categoryRepository.UpdateAsync(category);

            if(category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);

        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {


            var category = await _categoryRepository.DeleteAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);

        }

    }
}
