namespace ResourceBookingAPI.Interfaces.Services
{
    public interface ICdnService
    {
        Task<string?> Upload(IFormFile file);
        Task<bool> Delete(string filePath);
    }
}
