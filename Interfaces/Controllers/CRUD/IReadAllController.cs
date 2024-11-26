using Microsoft.AspNetCore.Mvc;

namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface IReadAllController
    {
        Task<IActionResult> GetAll([FromQuery] string id);
    }
}
