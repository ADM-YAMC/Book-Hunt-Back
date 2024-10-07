using ApplicationLayer.Services.Auth;
using DomainLayer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<Response<AuthAccount>> Login([FromBody] UserCredentials credentials)
            => await _authService.Authenticate(credentials);
        [HttpGet("refresh-token")]
        public async Task<Response<string>> RefreshToken(string token)
           => await _authService.RefreshToken(token);
    }
}
