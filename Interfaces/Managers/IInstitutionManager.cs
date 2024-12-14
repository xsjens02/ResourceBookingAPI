using ResourceBookingAPI.Interfaces.Managers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IInstitutionManager :
        ICreateManager<Institution>,
        IGetManager<Institution, string>,
        IUpdateManager<Institution, string>
    {
    }
}
