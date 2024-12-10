using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IErrorReportRepos :
        ICrudRepos<ErrorReport, string>
    {
        Task<bool> AnyActive(string resourceId);
    }
}
