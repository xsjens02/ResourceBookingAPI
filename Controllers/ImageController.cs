using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/images")]
    [AllowAnonymous]
    public class ImageController : ControllerBase, IImageController
    {
        private readonly ICdnService _cdnService;
        public ImageController(ICdnService cdnService) =>
            _cdnService = cdnService;

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null)
                return BadRequest("No file provided");

            var result = await _cdnService.Upload(file);
            if (result == null)
                return BadRequest("File upload failed");

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("Url cannot be null or empty");

            var result = await _cdnService.Delete(url);
            if (result)
                return Ok();

            return NotFound("File not found");
        }
    }
}
