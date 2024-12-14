using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IInstitutionController :
        IPostController<Institution>,
        IGetController<string>,
        IPutController<Institution, string>
    {
    }
}
