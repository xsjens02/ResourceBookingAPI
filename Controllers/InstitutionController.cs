using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/institutions")]
    [AllowAnonymous]
    //[Authorize(Roles = "admin")]
    public class InstitutionController : ControllerBase, IInstitutionController<Institution, string>
    {
        private readonly IInstitutionRepos<Institution, string> _institutionRepo;
        public InstitutionController(IInstitutionRepos<Institution, string> institutionRepo)
        {
            _institutionRepo = institutionRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Institution entity)
        {
            if (entity == null)
                return BadRequest("Institution entity cannot be null");

            try
            {
                await _institutionRepo.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create institution. Error:{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var institution = await _institutionRepo.Get(id);
            if (institution == null)
                return NotFound($"No institution found with Id:{id}");

            return Ok(institution);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Institution entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("Institution entity cannot be null");

            var succes = await _institutionRepo.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No institution found with Id:{id}");
        }
    }
}
