using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// Repository for managing ErrorReport entities in a MongoDB database.
    /// Implements CRUD operations for ErrorReports.
    /// </summary>
    public class ErrorReportMongoRepos : ICrudRepos<ErrorReport, string>
    {
        private readonly IMongoCollection<ErrorReport> _errorReports;

        /// <summary>
        /// Initializes the ErrorReport repository with a MongoDB collection.
        /// </summary>
        /// <param name="mongoService">Service for accessing MongoDB.</param>
        public ErrorReportMongoRepos(IMongoService mongoService)
        {
            _errorReports = mongoService.GetCollection<ErrorReport>("errorreports");
        }

        /// <summary>
        /// Retrieves a single error report by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the error report to retrieve.</param>
        /// <returns>The matching ErrorReport or null if not found.</returns>
        public async Task<ErrorReport> Get(string id)
        {
            return await _errorReports.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all error reports for a specific resource.
        /// </summary>
        /// <param name="resourceId">The ID of the resource associated with the error reports.</param>
        /// <returns>A list of error reports for the specified resource.</returns>
        public async Task<IEnumerable<ErrorReport>> GetAll([FromQuery] string resourceId)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.ResourceId, resourceId);
            return await _errorReports.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new error report in the database.
        /// </summary>
        /// <param name="entity">The ErrorReport entity to be created.</param>
        public async Task Create([FromBody] ErrorReport entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _errorReports.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing error report by its ID.
        /// </summary>
        /// <param name="id">The ID of the error report to update.</param>
        /// <param name="entity">The updated ErrorReport entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> Update(string id, [FromBody] ErrorReport entity)
        {
            entity.Id = id;

            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes an error report by its ID.
        /// </summary>
        /// <param name="id">The ID of the error report to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}