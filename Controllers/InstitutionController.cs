﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Controllers
{
    /// <summary>
    /// Controller for managing institutions.
    /// Includes routes for creating, retrieving, and updating institution data.
    /// </summary>
    [ApiController]
    [Route("api/institutions")]
    [AllowAnonymous]
    //[Authorize(Roles = "admin")]
    public class InstitutionController : ControllerBase, IInstitutionController
    {
        private readonly IInstitutionRepos _institutionRepo;

        /// <summary>
        /// Initializes the InstitutionController with the repository for institution management.
        /// </summary>
        /// <param name="institutionRepo">The repository used for managing institutions.</param>
        public InstitutionController(IInstitutionRepos institutionRepo)
        {
            _institutionRepo = institutionRepo;
        }

        /// <summary>
        /// Creates a new institution.
        /// </summary>
        /// <param name="entity">The institution entity to create.</param>
        /// <returns>An IActionResult indicating the result of the create operation.</returns>
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

        /// <summary>
        /// Gets an institution by its ID.
        /// </summary>
        /// <param name="id">The ID of the institution to retrieve.</param>
        /// <returns>An IActionResult containing the institution or an error message.</returns>
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

        /// <summary>
        /// Updates an existing institution.
        /// </summary>
        /// <param name="id">The ID of the institution to update.</param>
        /// <param name="entity">The updated institution entity.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
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