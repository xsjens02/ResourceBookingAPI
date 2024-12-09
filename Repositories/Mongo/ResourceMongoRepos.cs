using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// Repository for managing Resource entities in a MongoDB database.
    /// Implements basic CRUD operations for handling resources.
    /// </summary>
    public class ResourceMongoRepos : ICrudRepos<Resource, string>
    {
        private readonly IMongoCollection<Resource> _resources;

        /// <summary>
        /// Initializes the Resource repository with a MongoDB collection.
        /// </summary>
        /// <param name="mongoService">Service for accessing MongoDB.</param>
        public ResourceMongoRepos(IMongoService mongoService)
        {
            _resources = mongoService.GetCollection<Resource>("resources");
        }

        /// <summary>
        /// Retrieves a single resource by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the resource to retrieve.</param>
        /// <returns>The matching Resource or null if not found.</returns>
        public async Task<Resource> Get(string id)
        {
            return await _resources.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all resources for a given institution ID.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to filter resources by.</param>
        /// <returns>A list of resources belonging to the specified institution.</returns>
        public async Task<IEnumerable<Resource>> GetAll([FromQuery] string institutionId)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.InstitutionId, institutionId);
            return await _resources.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new resource in the database.
        /// </summary>
        /// <param name="entity">The Resource entity to be created.</param>
        public async Task Create([FromBody] Resource entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _resources.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to update.</param>
        /// <param name="entity">The updated Resource entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> Update(string id, [FromBody] Resource entity)
        {
            entity.Id = id;

            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a resource by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the resource to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}