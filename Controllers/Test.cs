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
            var apiKey = Environment.GetEnvironmentVariable("CDN_KEY");
            if (apiKey != null)
            {
                return "apikey is working!";
            } 

            var tesaedg = User.FindFirst(ClaimTypes.Role)?.Value;
            return "server is running";
        }
    }
}
