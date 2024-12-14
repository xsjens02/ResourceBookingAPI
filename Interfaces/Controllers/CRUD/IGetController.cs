using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IGetController<TKey>
    {
        Task<IActionResult> Get(TKey id);
    }
}
