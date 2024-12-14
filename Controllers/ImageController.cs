using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Managers;
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
        private readonly IImageManager _imageManager;

        /// <summary>
        /// Initializes the ImageController with the CDN manager for managing images.
        /// </summary>
        /// <param name="cdnService">The manager used for image uploads and deletions.</param>
        public ImageController(IImageManager imageManager) =>
            _imageManager = imageManager;

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

            var result = await _imageManager.UploadImage(file);
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

            var result = await _imageManager.DeleteImage(filePath);
            if (result)
                return Ok();

            return NotFound("File not found");
        }
    }
}