using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// Repository for managing Institution entities in a MongoDB database.
    /// Implements basic operations for handling institutions.
    /// </summary>
    public class InstitutionMongoRepos : IInstitutionRepos
    {
        private readonly IMongoCollection<Institution> _institutions;

        /// <summary>
        /// Initializes the Institution repository with a MongoDB collection.
        /// </summary>
        /// <param name="mongoService">Service for accessing MongoDB.</param>
        public InstitutionMongoRepos(IMongoService mongoService)
        {
            _institutions = mongoService.GetCollection<Institution>("institutions");
        }

        /// <summary>
        /// Creates a new institution in the database.
        /// </summary>
        /// <param name="entity">The Institution entity to be created.</param>
        public async Task Create([FromBody] Institution entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _institutions.InsertOneAsync(entity);
        }

        /// <summary>
        /// Retrieves a single institution by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the institution to retrieve.</param>
        /// <returns>The matching Institution or null if not found.</returns>
        public async Task<Institution> Get(string id)
        {
            return await _institutions.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing institution by its ID.
        /// </summary>
        /// <param name="id">The ID of the institution to update.</param>
        /// <param name="entity">The updated Institution entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> Update(string id, [FromBody] Institution entity)
        {
            entity.Id = id;

            var filter = Builders<Institution>.Filter.Eq(i => i.Id, id);
            var result = await _institutions.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }
    }
}