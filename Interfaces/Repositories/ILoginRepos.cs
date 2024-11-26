using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface ILoginRepos
    {
        Task<User> GetUser(string username);
        Task<bool> ValidatePassword(User user, string password);
    }
}
