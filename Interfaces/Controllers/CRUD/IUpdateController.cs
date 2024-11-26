using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IUpdateController<T, TKey>
    {
        Task<IActionResult> Update(TKey id, [FromBody] T entity);
    }
}
