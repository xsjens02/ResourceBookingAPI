using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// ResourceManager is responsible for managing resource-related operations,
    /// including creating, retrieving, updating, and deleting resources. It interacts
    /// with the resource repository to perform these operations.
    /// </summary>
    public class ResourceManager : IResourceManager
    {
        /// <summary>
        /// The repository instance used to access resource data.
        /// </summary>
        private readonly IResourceRepos _resourceRepos;

        /// <summary>
        /// Initializes a new instance of the ResourceManager class.
        /// </summary>
        /// <param name="resourceRepos">The repository to be used for resource operations.</param>
        public ResourceManager(IResourceRepos resourceRepos)
        {
            _resourceRepos = resourceRepos;
        }

        /// <summary>
        /// Creates a new resource in the system.
        /// </summary>
        /// <param name="entity">The resource entity to be created.</param>
        /// <returns>A task that represents the asynchronous creation operation.</returns>
        public async Task Create(Resource entity)
        {
            await _resourceRepos.Create(entity);
        }

        /// <summary>
        /// Deletes a resource by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to be deleted.</param>
        /// <returns>A task that represents the asynchronous deletion operation. 
        /// The task result is a boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            return await _resourceRepos.Delete(id);
        }

        /// <summary>
        /// Retrieves a resource by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to retrieve.</param>
        /// <returns>The requested resource entity.</returns>
        public async Task<Resource> Get(string id)
        {
            return await _resourceRepos.Read(id);
        }

        /// <summary>
        /// Retrieves all resources associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution whose resources to retrieve.</param>
        /// <returns>A collection of resources associated with the institution.</returns>
        public async Task<IEnumerable<Resource>> GetAll(string institutionId)
        {
            return await _resourceRepos.ReadAll(institutionId);
        }

        /// <summary>
        /// Updates an existing resource identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to update.</param>
        /// <param name="entity">The updated resource entity.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Update(string id, Resource entity)
        {
            return await _resourceRepos.Update(id, entity);
        }
    }
}