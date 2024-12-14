using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// InstitutionMongoRepos is responsible for interacting with the MongoDB database
    /// to perform CRUD operations for institution entities. It utilizes the MongoDB
    /// driver to access the "institutions" collection and manage institution data.
    /// </summary>
    public class InstitutionMongoRepos : IInstitutionRepos
    {
        /// <summary>
        /// The MongoDB collection used to access institution data.
        /// </summary>
        private readonly IMongoCollection<Institution> _institutions;

        /// <summary>
        /// Initializes a new instance of the InstitutionMongoRepos class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service to provide access to the "institutions" collection.</param>
        public InstitutionMongoRepos(IMongoService mongoService)
        {
            _institutions = mongoService.GetCollection<Institution>("institutions");
        }

        /// <summary>
        /// Creates a new institution in the database.
        /// If the institution already has an ID, it is set to null before insertion.
        /// </summary>
        /// <param name="entity">The institution entity to be created.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        public async Task Create(Institution entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _institutions.InsertOneAsync(entity);
        }

        /// <summary>
        /// Retrieves an institution by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the institution to retrieve.</param>
        /// <returns>The requested institution entity or null if not found.</returns>
        public async Task<Institution> Read(string id)
        {
            return await _institutions.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing institution identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the institution to update.</param>
        /// <param name="entity">The updated institution entity.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> Update(string id, Institution entity)
        {
            entity.Id = id;

            var filter = Builders<Institution>.Filter.Eq(i => i.Id, id);
            var result = await _institutions.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }
    }
}