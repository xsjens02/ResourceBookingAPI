using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Test : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var tesaedg = User.FindFirst(ClaimTypes.Role)?.Value;
            return "server is running";
        }
    }
}
