using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// ErrorReportManager is responsible for managing error report operations.
    /// It interacts with the error report repository to handle CRUD operations
    /// and perform specific business logic related to error reports.
    /// </summary>
    public class ErrorReportManager : IErrorReportManager
    {
        /// <summary>
        /// The repository instance used to access error report data.
        /// </summary>
        private readonly IErrorReportRepos _errorReportRepo;

        /// <summary>
        /// Initializes a new instance of the ErrorReportManager class.
        /// </summary>
        /// <param name="errorReportRepo">The error report repository to be used by this manager.</param>
        public ErrorReportManager(IErrorReportRepos errorReportRepo)
        {
            _errorReportRepo = errorReportRepo;
        }

        /// <summary>
        /// Checks if there are any active error reports associated with a specific resource.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource.</param>
        /// <returns>A boolean indicating whether any active error reports exist for the resource.</returns>
        public async Task<bool> AnyActiveOnResource(string resourceId)
        {
            var allReports = await _errorReportRepo.ReadAllActiveOnResource(resourceId);
            return allReports.Any();
        }

        /// <summary>
        /// Clears all unresolved error reports for a specific resource by marking them as resolved.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource.</param>
        /// <returns>A boolean indicating whether any error reports were successfully resolved.</returns>
        public async Task<bool> ClearUnresolvedOnResource(string resourceId)
        {
            return await _errorReportRepo.ResolveOnResource(resourceId);
        }

        /// <summary>
        /// Creates a new error report in the system.
        /// </summary>
        /// <param name="entity">The error report entity to create.</param>
        public async Task Create(ErrorReport entity)
        {
            await _errorReportRepo.Create(entity);
        }

        /// <summary>
        /// Deletes an error report by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to delete.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            return await _errorReportRepo.Delete(id);
        }

        /// <summary>
        /// Retrieves an error report by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to retrieve.</param>
        /// <returns>The requested error report entity.</returns>
        public async Task<ErrorReport> Get(string id)
        {
            return await _errorReportRepo.Read(id);
        }

        /// <summary>
        /// Retrieves all error reports associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution.</param>
        /// <returns>A collection of error reports associated with the institution.</returns>
        public async Task<IEnumerable<ErrorReport>> GetAll(string institutionId)
        {
            return await _errorReportRepo.ReadAll(institutionId);
        }

        /// <summary>
        /// Updates an existing error report identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the error report to update.</param>
        /// <param name="entity">The updated error report entity.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Update(string id, ErrorReport entity)
        {
            return await _errorReportRepo.Update(id, entity);
        }
    }
}