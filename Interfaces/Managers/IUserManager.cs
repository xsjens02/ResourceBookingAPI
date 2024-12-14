using ResourceBookingAPI.Interfaces.Managers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IUserManager :
        ICrudManager<User, string>
    {
        Task<User> GetUserFromUsername(string username);
        bool ValidatePassword(User user, string password);
    }
}
