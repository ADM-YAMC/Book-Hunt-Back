
using ApplicationLayer.Services.UserServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService) 
        { 
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Response<User>>> GetUsersAsync()
            => await _userService.GetUsersAsync();
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<User>>> GetUserByIdAsync(int id)
            => await _userService.GetUserByIdAsync(id);
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddUserAsync(User user)
            => await _userService.AddUserAsync(user);
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> UpdateUserAsync(int id, User user)
            => await _userService.UpdateUserAsync(id, user);

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<Response<string>> DeleteUserAsync(int id)
            => await _userService.DeleteUserAsync(id);
        
    }
}
