using ApplicationLayer.Services.CategoryServices;
using ApplicationLayer.Services.UserServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;
        public CategoryController(CategoryService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<Response<Category>>> GetCategoryAsync()
            => await _service.GetCategoryAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Category>>> GetCategoryByIdAsync(int id)
            => await _service.GetCategoryByIdAsync(id);
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddCategoryAsync(Category entry)
            => await _service.AddCategoryAsync(entry);
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> UpdateCategoryAsync(int id, Category entry)
            => await _service.UpdateCategoryAsync(id, entry);

        [HttpDelete("{id}")]
        public async Task<Response<string>> DeleteCategoryAsync(int id)
            => await _service.DeleteCategoryAsync(id);
    }
}
