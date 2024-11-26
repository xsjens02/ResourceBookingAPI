using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/resources")]
    [Authorize]
    public class ResourceController : ControllerBase, ICrudController<Resource, string>
    {
        private readonly ICrudRepos<Resource, string> _resourceRepo;
        public ResourceController(ICrudRepos<Resource, string> resourceRepo)
        {
            _resourceRepo = resourceRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var resource = await _resourceRepo.Get(id);
            if (resource == null)
                return NotFound($"No resource found with Id:{id}");

            return Ok(resource);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string institutionId)
        {
            if (string.IsNullOrWhiteSpace(institutionId))
                return BadRequest("InstitutionId cannot be null");

            var resources = await _resourceRepo.GetAll(institutionId);
            return Ok(resources);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] Resource entity)
        {
            if (entity == null)
                return BadRequest("Resource entity cannot be null");

            try
            {
                await _resourceRepo.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create resoure. Error:{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string id, [FromBody] Resource entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Resource entity cannot be null");

            var succes = await _resourceRepo.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No resource found with Id:{id}");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _resourceRepo.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No resource found with Id:{id}");
        }
    }
}
