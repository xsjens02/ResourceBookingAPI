using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IUpdateRepos<T, TKey>
    {
        Task<bool> Update(TKey id, [FromBody] T entity);
    }
}
