using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for handling user login and token generation.
    /// Includes a route for logging in.
    /// </summary>
    [ApiController]
    [Route("api/login")]
    [AllowAnonymous]
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes the LoginController with the JWT service for handling login and token generation.
        /// </summary>
        /// <param name="jwtService">The service used to generate JWT tokens.</param>
        public LoginController(IJwtService jwtService) =>
            _jwtService = jwtService;

        /// <summary>
        /// Generates a JWT token based on the provided login credentials.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>An IActionResult with a JWT token if login is successful, or Unauthorized if login fails.</returns>
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