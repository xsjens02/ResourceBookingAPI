using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// InstitutionManager is responsible for managing institution-related operations, 
    /// including creating, retrieving, and updating institution data. It interacts 
    /// with the institution repository to perform these operations.
    /// </summary>
    public class InstitutionManager : IInstitutionManager
    {
        /// <summary>
        /// The repository instance used to access institution data.
        /// </summary>
        private readonly IInstitutionRepos _institutionRepos;

        /// <summary>
        /// Initializes a new instance of the InstitutionManager class.
        /// </summary>
        /// <param name="institutionRepos">The repository to be used for institution operations.</param>
        public InstitutionManager(IInstitutionRepos institutionRepos)
        {
            _institutionRepos = institutionRepos;
        }

        /// <summary>
        /// Creates a new institution in the system.
        /// </summary>
        /// <param name="entity">The institution entity to be created.</param>
        /// <returns>A task that represents the asynchronous creation operation.</returns>
        public async Task Create(Institution entity)
        {
            await _institutionRepos.Create(entity);
        }

        /// <summary>
        /// Retrieves an institution by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the institution to retrieve.</param>
        /// <returns>The requested institution entity.</returns>
        public async Task<Institution> Get(string id)
        {
            return await _institutionRepos.Read(id);
        }

        /// <summary>
        /// Updates an existing institution identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the institution to update.</param>
        /// <param name="entity">The updated institution entity.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Update(string id, Institution entity)
        {
            return await _institutionRepos.Update(id, entity);
        }
    }
}