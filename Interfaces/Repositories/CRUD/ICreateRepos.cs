using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface ICreateRepos<T>
    {
        Task Create([FromBody] T entity);
    }
}
