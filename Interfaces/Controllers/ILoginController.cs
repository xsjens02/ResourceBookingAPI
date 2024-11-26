using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.DTOs;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface ILoginController
    {
        Task<IActionResult> login([FromBody] LoginRequestDto request);
    }
}
