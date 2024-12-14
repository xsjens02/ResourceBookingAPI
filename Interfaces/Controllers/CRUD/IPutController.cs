using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IPutController<T, TKey>
    {
        Task<IActionResult> Put(TKey id, [FromBody] T entity);
    }
}
