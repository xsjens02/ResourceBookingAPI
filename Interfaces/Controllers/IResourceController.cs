using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IResourceController :
        ICrudController<Resource, string>
    {
    }
}
