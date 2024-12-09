using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing bookings.
    /// Includes routes for retrieving, creating, updating, and deleting bookings.
    /// </summary>
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingController : ControllerBase, IBookingController
    {
        private readonly IBookingRepos _bookingRepo;

        /// <summary>
        /// Initializes the BookingController with a repository for interacting with bookings.
        /// </summary>
        /// <param name="bookingRepo">The repository used to manage bookings.</param>
        public BookingController(IBookingRepos bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        /// <summary>
        /// Retrieves a booking by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The user ID used to filter bookings.</param>
        /// <returns>An IActionResult containing the list of bookings for the user.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId cannot be null");

            var bookings = await _bookingRepo.GetAll(userId);
            return Ok(bookings);
        }

        /// <summary>
        /// Retrieves booking statistics within a date range for an institution.
        /// Accessible only to users with the "admin" role.
        /// </summary>
        /// <param name="statRequest">The statistics request containing the date range and institution ID.</param>
        /// <returns>An IActionResult with the list of bookings within the date range.</returns>
        [HttpPost("statistic")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetStatistic([FromBody] BookingStatRequestDto statRequest)
        {
            if (statRequest == null)
                return BadRequest("Request cannot be null");

            if (string.IsNullOrWhiteSpace(statRequest.InstitutionId))
                return BadRequest("InstitutionId cannot be null or empty");

            var bookings = await _bookingRepo.GetAllWithinDates(statRequest.StartDate, statRequest.EndTime, statRequest.InstitutionId);
            return Ok(bookings);
        }

        /// <summary>
        /// Retrieves all pending bookings for a user.
        /// </summary>
        /// <param name="pendingRequest">The request containing the user ID and current date.</param>
        /// <returns>An IActionResult with the list of pending bookings for the user.</returns>
        [HttpPost("pending")]
        public async Task<IActionResult> GetPending([FromBody] BookingPendingRequestDto pendingRequest)
        {
            if (pendingRequest == null)
                return BadRequest("Request cannot be null");

            if (string.IsNullOrWhiteSpace(pendingRequest.UserId))
                return BadRequest("UserId cannot be null or empty");

            var bookings = await _bookingRepo.GetAllPending(pendingRequest.UserId, pendingRequest.CurrentDate);
            return Ok(bookings);
        }

        /// <summary>
        /// Retrieves all bookings for a resource on a specific date.
        /// </summary>
        /// <param name="resourceRequest">The request containing the resource ID and date.</param>
        /// <returns>An IActionResult with the list of bookings for the resource on the specified date.</returns>
        [HttpPost("resourcebookings")]
        public async Task<IActionResult> GetResourceBookings([FromBody] ResourceBookingsRequestDto resourceRequest)
        {
            if (resourceRequest == null)
                return BadRequest("Request cannot be null");

            if (string.IsNullOrWhiteSpace(resourceRequest.ResourceId))
                return BadRequest("ResourceId cannot be null or empty");

            var bookings = await _bookingRepo.GetAllResourceBookings(resourceRequest.ResourceId, resourceRequest.Date);
            return Ok(bookings);
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="entity">The booking entity to create.</param>
        /// <returns>An IActionResult indicating the result of the creation operation.</returns>
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

        /// <summary>
        /// Updates an existing booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="entity">The updated booking entity.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Booking entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Booking entity cannot be null");

            var success = await _bookingRepo.Update(id, entity);
            if (success)
                return NoContent();

            return NotFound($"No booking found with Id:{id}");
        }

        /// <summary>
        /// Deletes a booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>An IActionResult indicating the result of the deletion operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var success = await _bookingRepo.Delete(id);
            if (success)
                return NoContent();

            return NotFound($"No booking found with Id:{id}");
        }
    }
}