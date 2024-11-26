using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IReadAllRepos<T>
    {
        Task<IEnumerable<T>> GetAll([FromQuery] string id);
    }
}
