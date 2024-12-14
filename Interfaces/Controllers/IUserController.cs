using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IUserController : 
        ICrudController<User, string>
    {
    }
}
