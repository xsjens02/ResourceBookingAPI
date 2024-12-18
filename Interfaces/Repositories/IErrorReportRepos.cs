using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IErrorReportRepos :
        ICrudRepos<ErrorReport, string>
    {
        Task<IEnumerable<ErrorReport>> ReadAllActiveOnResource(string resourceId);
        Task<bool> ResolveOnResource(string resourceId);
    }
}
