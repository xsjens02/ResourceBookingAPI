using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IGetAllController
    {
        Task<IActionResult> GetAll([FromQuery] string id);
    }
}
