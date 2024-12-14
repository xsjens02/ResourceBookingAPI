using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IPostController<T>
    {
        Task<IActionResult> Post([FromBody] T entity);
    }
}
