using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing error reports.
    /// Includes routes for retrieving, creating, updating, and deleting error reports.
    /// </summary>
    [ApiController]
    [Route("api/error-reports")]
    [Authorize]
    public class ErrorReportController : ControllerBase, IErrorReportController
    {
        private readonly IErrorReportManager _errorReportManager;

        /// <summary>
        /// Initializes the ErrorReportController with a manager for interacting with error reports.
        /// </summary>
        /// <param name="errorReportRepo">The manager used to manage error reports.</param>
        public ErrorReportController(IErrorReportManager errorReportManager)
        {
            this._errorReportManager = errorReportManager;
        }

        /// <summary>
        /// Retrieves an error report by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the error report to retrieve.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var errorReport = await _errorReportManager.Get(id);
            if (errorReport == null)
                return NotFound($"No error report found with Id:{id}");

            return Ok(errorReport);
        }

        /// <summary>
        /// Retrieves all error reports for a specified institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution whose error reports are to be retrieved.</param>
        /// <returns>An IActionResult containing the list of error reports for the institution.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string institutionId)
        {
            if (string.IsNullOrWhiteSpace(institutionId))
                return BadRequest("InstitutionId cannot be null");

            var errorReports = await _errorReportManager.GetAll(institutionId);
            return Ok(errorReports);
        }

        /// <summary>
        /// Checks whether there are any active (unresolved) error reports for a specific resource.
        /// </summary>
        /// <param name="resourceId">The ID of the resource to check for active error reports.</param>
        /// <returns>
        /// An IActionResult containing a boolean value:
        /// - `true` if there are active error reports for the specified resource.
        /// - `false` if no active error reports exist.
        /// </returns>
        [HttpGet("active")]
        public async Task<IActionResult> GetResourceStatus([FromQuery] string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return BadRequest("ResourceId cannot be null");

            var activeReports = await _errorReportManager.AnyActiveOnResource(resourceId);
            return Ok(activeReports);
        }

        /// <summary>
        /// Creates a new error report.
        /// </summary>
        /// <param name="entity">The error report entity to create.</param>
        /// <returns>An IActionResult indicating the result of the creation operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ErrorReport entity)
        {
            if (entity == null)
                return BadRequest("Error report entity cannot be null");

            try
            {
                await _errorReportManager.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create error report. Error:{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing error report by its ID.
        /// Accessible only to users with the "admin" role.
        /// </summary>
        /// <param name="id">The ID of the error report to update.</param>
        /// <param name="entity">The updated error report entity.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(string id, [FromBody] ErrorReport entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Error report entity cannot be null");

            var success = await _errorReportManager.Update(id, entity);
            if (success)
                return NoContent();

            return NotFound($"No error report found with Id:{id}");
        }

        /// <summary>
        /// Deletes an error report by its ID.
        /// Accessible only to users with the "admin" role.
        /// </summary>
        /// <param name="id">The ID of the error report to delete.</param>
        /// <returns>An IActionResult indicating the result of the deletion operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var success = await _errorReportManager.Delete(id);
            if (success)
                return NoContent();

            return NotFound($"No error report found with Id:{id}");
        }
    }
}