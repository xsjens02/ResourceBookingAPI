using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IDeleteRepos<TKey>
    {
        Task<bool> Delete(TKey id);
    }
}
