using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// ImageManager is responsible for managing image-related operations, 
    /// such as uploading and deleting images. It interacts with a CDN service 
    /// to perform the actual file operations.
    /// </summary>
    public class ImageManager : IImageManager
    {
        /// <summary>
        /// The CDN service instance used to handle image upload and deletion operations.
        /// </summary>
        private readonly ICdnService _cdnService;

        /// <summary>
        /// Initializes a new instance of the ImageManager class.
        /// </summary>
        /// <param name="cdnService">The CDN service to be used for uploading and deleting images.</param>
        public ImageManager(ICdnService cdnService)
        {
            _cdnService = cdnService;
        }

        /// <summary>
        /// Deletes an image from the CDN.
        /// </summary>
        /// <param name="filePath">The file path of the image to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is 
        /// a boolean indicating whether the image deletion was successful.</returns>
        public async Task<bool> DeleteImage(string filePath)
        {
            return await _cdnService.Delete(filePath);
        }

        /// <summary>
        /// Uploads an image to the CDN.
        /// </summary>
        /// <param name="file">The image file to be uploaded.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is 
        /// the URL of the uploaded image, or null if the upload fails.</returns>
        public async Task<string?> UploadImage(IFormFile file)
        {
            return await _cdnService.Upload(file);
        }
    }
}