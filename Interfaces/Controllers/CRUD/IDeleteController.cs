using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IDeleteController<TKey>
    {
        Task<IActionResult> Delete(TKey id);
    }
}
