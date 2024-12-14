using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// Includes routes for retrieving, creating, updating, and deleting users.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserManager _userManager;

        /// <summary>
        /// Initializes the UserController with the user manager for managing users.
        /// </summary>
        /// <param name="userRepo">The manager used for CRUD operations on users.</param>
        public UserController(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>An IActionResult containing the user if found, or NotFound if no user exists with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var user = await _userManager.Get(id);
            if (user == null)
                return NotFound($"No user found with Id:{id}");

            return Ok(user);
        }

        /// <summary>
        /// Retrieves all users for a specified institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution whose users are to be retrieved.</param>
        /// <returns>An IActionResult containing the list of users for the given institution.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string institutionId)
        {
            if (string.IsNullOrWhiteSpace(institutionId))
                return BadRequest("InstitutionId cannot be null");

            var users = await _userManager.GetAll(institutionId);
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="entity">The user entity to create.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User entity)
        {
            if (entity == null)
                return BadRequest("User entity cannot be null");

            try
            {
                await _userManager.Create(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create user. Error:{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="entity">The updated user entity.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            if (entity == null)
                return BadRequest("User entity cannot be null");

            var succes = await _userManager.Update(id, entity);
            if (succes)
                return NoContent();

            return NotFound($"No user found with Id:{id}");
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty");

            var succes = await _userManager.Delete(id);
            if (succes)
                return NoContent();

            return NotFound($"No user found with Id:{id}");
        }
    }
}