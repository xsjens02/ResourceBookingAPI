using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IReadRepos<T, TKey>
    {
        Task<T> Get(TKey id);
    }
}
