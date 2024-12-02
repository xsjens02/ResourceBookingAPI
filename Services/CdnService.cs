using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Services
{
    public class CdnService : ICdnService
    {
        public Task<bool> DeleteFile(string url)
        {
            throw new NotImplementedException();
            //
        }

        public Task<string> UploadFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
