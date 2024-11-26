using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface ICreateController<T>
    {
        Task<IActionResult> Create([FromBody] T entity);
    }
}
