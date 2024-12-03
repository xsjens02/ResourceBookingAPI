using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface ILoginRepos
    {
        Task<User> FetchUser(string username);
        bool ValidatePassword(User user, string password);
    }
}
