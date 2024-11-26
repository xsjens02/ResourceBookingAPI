using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingController : ControllerBase, ICrudController<Booking, string>
    {
        private readonly ICrudRepos<Booking, string> _bookingRepo;
        public BookingController(ICrudRepos<Booking, string> bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var booking = await _bookingRepo.Get(id);
            if (booking == null)
                return NotFound($"No booking found with Id:{id}");

            return Ok(booking);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId cannot be null");

            var bookings = await _bookingRepo.GetAll(userId);
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Booking entity)
        {
            if (entity == null)
                return BadRequest("Booking entity cannot be null");

            try
            {
                await _bookingRepo.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create booking. Error:{ex.Message}"); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Booking entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Booking entity cannot be null");

            var succes = await _bookingRepo.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No booking found with Id:{id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _bookingRepo.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No booking found with Id:{id}");
        }
    }
}
