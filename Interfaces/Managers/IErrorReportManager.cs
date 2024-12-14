using ResourceBookingAPI.Interfaces.Managers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IErrorReportManager :
        ICrudManager<ErrorReport, string>
    {
        Task<bool> AnyActiveOnResource(string resourceId);
    }
}
