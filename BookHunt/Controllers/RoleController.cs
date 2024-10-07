using ApplicationLayer.Services.RoleServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;
        public RoleController(RoleService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Response<Role>>> GetRoleAsync()
            => await _service.GetRoleAllAsync();
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<Role>>> GetRoleByIdAsync(int id)
            => await _service.GetRoleByIdAsync(id);
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddRoleAsync(Role entry)
            => await _service.AddRoleAsync(entry);
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> UpdateRoleAsync(int id, Role entry)
            => await _service.UpdateRoleAsync(id, entry);

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<Response<string>> DeleteRoleAsync(int id)
            => await _service.DeleteRoleAsync(id);
    }
}
