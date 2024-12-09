using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing images.
    /// Includes routes for uploading and deleting images.
    /// </summary>
    [ApiController]
    [Route("api/images")]
    [Authorize(Roles = "admin")]
    public class ImageController : ControllerBase, IImageController
    {
        private readonly ICdnService _cdnService;

        /// <summary>
        /// Initializes the ImageController with the CDN service for managing images.
        /// </summary>
        /// <param name="cdnService">The service used for image uploads and deletions.</param>
        public ImageController(ICdnService cdnService) =>
            _cdnService = cdnService;

        /// <summary>
        /// Uploads an image file to the CDN.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <returns>An IActionResult indicating the result of the upload operation.</returns>
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

        /// <summary>
        /// Deletes an image file from the CDN.
        /// </summary>
        /// <param name="filePath">The URL or path of the image to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("Url cannot be null or empty");

            var result = await _cdnService.Delete(filePath);
            if (result)
                return Ok();

            return NotFound("File not found");
        }
    }
}