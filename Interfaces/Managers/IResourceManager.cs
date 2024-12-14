using ResourceBookingAPI.Interfaces.Managers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IResourceManager :
        ICrudManager<Resource, string>
    {
    }
}
