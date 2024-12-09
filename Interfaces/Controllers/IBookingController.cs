using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IBookingController : 
        ICrudController<Booking, string>
    {
        Task<IActionResult> GetStatistic([FromBody]BookingStatRequestDto statRequest);
        Task<IActionResult> GetPending([FromBody] BookingPendingRequestDto pendingRequest);
        Task<IActionResult> GetResourceBookings([FromBody] ResourceBookingsRequestDto resourceRequest);
    }
}
