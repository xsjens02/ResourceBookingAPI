using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IReadController<TKey>
    {
        Task<IActionResult> Get(TKey id);
    }
}
