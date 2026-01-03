using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.Controllers
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
        [HttpPost]
        [Route("CreateCateogry")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO requestDTO)
        {
            //Map Dto to Domain model
            var category = new Category
            {
                Name = requestDTO.Name,
                UrlHandle = requestDTO.UrlHandle
            };

            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle

            };
            
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllCategories")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            //Map domain model to Dto

            var response = new List<CategoryDto>();

            foreach(var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id=category.Id,
                    Name=category.Name,
                    UrlHandle=category.UrlHandle
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetCategoryById/{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _categoryRepository.GetById(id);
            if(category is null)
            {
                return NotFound();
            }
            
                var response = new CategoryDto
                {
                    Id=category.Id,
                    Name=category.Name,
                    UrlHandle=category.UrlHandle
                };

                return Ok(response);
        }

        [HttpPut]
        [Route("UpdateCategory/{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto categoryRequestDto)
        {
            var category = new Category
            {
                Id = id,
                Name = categoryRequestDto.Name,
                UrlHandle = categoryRequestDto.UrlHandle
            };

            category = await _categoryRepository.UpdateAsync(category);

            if(category is null)
            {
                return NotFound();
            }

            //Convert Domain model to Dto
            var response = new Category
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteCategoryById/{id:Guid}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] Guid id)
        {
           var category = await _categoryRepository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }

            //convert domain model to Dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
    }
}
