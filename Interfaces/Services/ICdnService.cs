namespace ResourceBookingAPI.Interfaces.Services
{
    public interface ICdnService
    {
        Task<string> UploadFile(IFormFile file);
        Task<bool> DeleteFile(string url);
    }
}
