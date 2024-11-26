using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    //[Authorize]
    public class UserController : ControllerBase, ICrudController<User, string>
    {
        private readonly ICrudRepos<User, string> _userRepo;
        public UserController(ICrudRepos<User, string> userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var user = await _userRepo.Get(id);
            if (user == null)
                return NotFound($"No user found with Id:{id}");

            return Ok(user);
        }

        [HttpGet("all")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll([FromQuery] string institutionId)
        {
            if (string.IsNullOrWhiteSpace(institutionId))
                return BadRequest("InstitutionId cannot be null");

            var users = await _userRepo.GetAll(institutionId);
            return Ok(users);
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] User entity)
        {
            if (entity == null)
                return BadRequest("User entity cannot be null");

            try
            {
                await _userRepo.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to user. Error:{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("User entity cannot be null");

            var succes = await _userRepo.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No user found with Id:{id}");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _userRepo.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No user found with Id:{id}");
        }
    }
}
