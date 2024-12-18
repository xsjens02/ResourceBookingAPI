using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// ErrorReportMongoRepos is responsible for interacting with the MongoDB database
    /// to perform CRUD operations for error report entities. It utilizes the MongoDB
    /// driver to access the "errorreports" collection and manage error report data.
    /// </summary>
    public class ErrorReportMongoRepos : IErrorReportRepos
    {
        /// <summary>
        /// The MongoDB collection used to access error report data.
        /// </summary>
        private readonly IMongoCollection<ErrorReport> _errorReports;

        /// <summary>
        /// Initializes a new instance of the ErrorReportMongoRepos class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service to provide access to the "errorreports" collection.</param>
        public ErrorReportMongoRepos(IMongoService mongoService)
        {
            _errorReports = mongoService.GetCollection<ErrorReport>("errorreports");
        }

        /// <summary>
        /// Retrieves an error report by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to retrieve.</param>
        /// <returns>The requested error report entity or null if not found.</returns>
        public async Task<ErrorReport> Read(string id)
        {
            return await _errorReports.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all error reports associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution whose error reports to retrieve.</param>
        /// <returns>A collection of error reports associated with the institution.</returns>
        public async Task<IEnumerable<ErrorReport>> ReadAll(string institutionId)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.InstitutionId, institutionId);
            return await _errorReports.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new error report in the database.
        /// If the error report already has an ID, it is set to null before insertion.
        /// </summary>
        /// <param name="entity">The error report entity to be created.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        public async Task Create(ErrorReport entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _errorReports.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing error report identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to update.</param>
        /// <param name="entity">The updated error report entity.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> Update(string id, ErrorReport entity)
        {
            entity.Id = id;

            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes an error report by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves all active error reports associated with a specific resource.
        /// Active error reports are those that have not been marked as resolved.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource whose active error reports to retrieve.</param>
        /// <returns>A collection of active error reports associated with the resource.</returns>
        public async Task<IEnumerable<ErrorReport>> ReadAllActiveOnResource(string resourceId)
        {
            return await _errorReports.Find(e => e.ResourceId == resourceId && !e.Resolved).ToListAsync();
        }

        /// <summary>
        /// Marks all unresolved error reports for a specific resource as resolved.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource whose error reports to resolve.</param>
        /// <returns>A boolean indicating whether any reports were successfully resolved.</returns>
        public async Task<bool> ResolveOnResource(string resourceId)
        {
            var filter = Builders<ErrorReport>.Filter.Where(e => e.ResourceId == resourceId && !e.Resolved);
            var update = Builders<ErrorReport>.Update.Set(e => e.Resolved, true);

            var result = await _errorReports.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}