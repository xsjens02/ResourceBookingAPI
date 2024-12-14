using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing resources.
    /// Includes routes for retrieving, creating, updating, and deleting resources.
    /// </summary>
    [ApiController]
    [Route("api/resources")]
    [Authorize]
    public class ResourceController : ControllerBase, IResourceController
    {
        private readonly IResourceManager _resourceManager;

        /// <summary>
        /// Initializes the ResourceController with the resource manager for managing resources.
        /// </summary>
        /// <param name="resourceRepo">The manager used for CRUD operations on resources.</param>
        public ResourceController(IResourceManager resourceManager)
        {
            this._resourceManager = resourceManager;
        }

        /// <summary>
        /// Retrieves a resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to retrieve.</param>
        /// <returns>An IActionResult containing the resource if found, or NotFound if no resource exists with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var resource = await _resourceManager.Get(id);
            if (resource == null)
                return NotFound($"No resource found with Id:{id}");

            return Ok(resource);
        }

        /// <summary>
        /// Retrieves all resources for a specified institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution whose resources are to be retrieved.</param>
        /// <returns>An IActionResult containing the list of resources for the given institution.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string institutionId)
        {
            if (string.IsNullOrWhiteSpace(institutionId))
                return BadRequest("InstitutionId cannot be null");

            var resources = await _resourceManager.GetAll(institutionId);
            return Ok(resources);
        }

        /// <summary>
        /// Creates a new resource.
        /// </summary>
        /// <param name="entity">The resource entity to create.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] Resource entity)
        {
            if (entity == null)
                return BadRequest("Resource entity cannot be null");

            try
            {
                await _resourceManager.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create resource. Error:{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to update.</param>
        /// <param name="entity">The updated resource entity.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(string id, [FromBody] Resource entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Resource entity cannot be null");

            var succes = await _resourceManager.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No resource found with Id:{id}");
        }

        /// <summary>
        /// Deletes a resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _resourceManager.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No resource found with Id:{id}");
        }
    }
}