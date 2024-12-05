using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IInstitutionRepos :
        ICreateRepos<Institution>,
        IReadRepos<Institution, string>,
        IUpdateRepos<Institution, string>
    {
    }
}
