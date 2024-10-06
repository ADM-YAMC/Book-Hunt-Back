using ApplicationLayer.Services.AuthorServices;
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
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _service;
        public AuthorController(AuthorService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<Response<Author>>> GetAuthorAsync()
            => await _service.GetAuthorAllAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Author>>> GetAuthorByIdAsync(int id)
            => await _service.GetAuthorByIdAsync(id);
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddAuthorAsync(Author entry)
            => await _service.AddAuthorAsync(entry);
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> UpdateAuthorAsync(int id, Author entry)
            => await _service.UpdateAuthorAsync(id, entry);

        [HttpDelete("{id}")]
        public async Task<Response<string>> DeleteAuthorAsync(int id)
            => await _service.DeleteAuthorAsync(id);
    }
}
