using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for testing server connectivity.
    /// Includes a route for checking if the server is running.
    /// </summary>
    [ApiController]
    [Route("api/test")]
    [AllowAnonymous]
    public class TestConnectionController : ControllerBase
    {
        /// <summary>
        /// Checks if the server is running by returning a simple message.
        /// </summary>
        /// <returns>A string message indicating the server is running.</returns>
        [HttpGet]
        public string Get()
        {
            return "server is running";
        }
    }
}