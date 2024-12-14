namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IImageManager
    {
        Task<string?> UploadImage(IFormFile file);
        Task<bool> DeleteImage(string filePath);
    }
}
