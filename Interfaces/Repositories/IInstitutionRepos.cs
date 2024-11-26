using ResourceBookingAPI.Interfaces.Repositories.CRUD;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IInstitutionRepos<T, TKey> :
        IReadRepos<T, TKey>,
        IUpdateRepos<T, TKey>
    {
    }
}
