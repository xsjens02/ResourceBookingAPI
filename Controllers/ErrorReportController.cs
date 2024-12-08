using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Handles booking-related operations such as creating, retrieving, updating and deleting bookings.
    /// </summary>
    [ApiController]
    [Route("api/error-reports")]
    [Authorize]
    public class ErrorReportController : ControllerBase, ICrudController<ErrorReport, string>
    {
        private readonly ICrudRepos<ErrorReport, string> _errorReportRepo;
        public ErrorReportController(ICrudRepos<ErrorReport, string> errorReportRepo)
        {
            _errorReportRepo = errorReportRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var errorReport = await _errorReportRepo.Get(id);
            if (errorReport == null)
                return NotFound($"No error report found with Id:{id}");

            return Ok(errorReport);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return BadRequest("ResourceId cannot be null");

            var errorReports = await _errorReportRepo.GetAll(resourceId);
            return Ok(errorReports); 
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ErrorReport entity)
        {
            if (entity == null)
                return BadRequest("Error report entity cannot be null");

            try
            {
                await _errorReportRepo.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create error report. Error:{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ErrorReport entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Error report entity cannot be null");

            var succes = await _errorReportRepo.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No error report found with Id:{id}");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _errorReportRepo.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No error report found with Id:{id}");
        }
    }
}
