using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    [AllowAnonymous]
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly IJwtService _jwtService;
        public LoginController(IJwtService jwtService) =>
            _jwtService = jwtService;

        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequestDto request)
        {
            var result = await _jwtService.GenerateToken(request);
            if (result == null) 
                return Unauthorized();

            return Ok(result);
        }
    }
}
