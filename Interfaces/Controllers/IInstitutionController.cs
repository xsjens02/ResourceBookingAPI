using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IInstitutionController :
        ICreateController<Institution>,
        IReadController<string>,
        IUpdateController<Institution, string>
    {
    }
}
