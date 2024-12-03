using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IImageController
    {
        Task<IActionResult> Upload([FromForm] IFormFile file);
        Task<IActionResult> Delete(string url);
    }
}
