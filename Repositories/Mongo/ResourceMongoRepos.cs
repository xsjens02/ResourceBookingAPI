using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// ResourceMongoRepos is responsible for performing CRUD operations on the "resources" collection in MongoDB.
    /// It interacts with MongoDB to manage resource entities.
    /// </summary>
    public class ResourceMongoRepos : IResourceRepos
    {
        /// <summary>
        /// The MongoDB collection used to access resource data.
        /// </summary>
        private readonly IMongoCollection<Resource> _resources;

        /// <summary>
        /// Initializes a new instance of the ResourceMongoRepos class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service used to access the "resources" collection.</param>
        public ResourceMongoRepos(IMongoService mongoService)
        {
            _resources = mongoService.GetCollection<Resource>("resources");
        }

        /// <summary>
        /// Retrieves a resource by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to retrieve.</param>
        /// <returns>The requested resource entity or null if not found.</returns>
        public async Task<Resource> Read(string id)
        {
            return await _resources.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all resources associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution to filter resources by.</param>
        /// <returns>A collection of resources associated with the specified institution.</returns>
        public async Task<IEnumerable<Resource>> ReadAll(string institutionId)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.InstitutionId, institutionId);
            return await _resources.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new resource in the database.
        /// If the resource already has an ID, it is set to null before insertion.
        /// </summary>
        /// <param name="entity">The resource entity to be created.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        public async Task Create(Resource entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _resources.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing resource identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to update.</param>
        /// <param name="entity">The updated resource entity.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> Update(string id, Resource entity)
        {
            entity.Id = id;

            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a resource from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to delete.</param>
        /// <returns>A boolean indicating whether the delete operation was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}