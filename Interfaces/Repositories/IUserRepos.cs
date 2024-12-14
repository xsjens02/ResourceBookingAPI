using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IUserRepos :
        ICrudRepos<User, string>
    {
        Task<User> ReadOnUsername(string username);
    }
}
