using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// UserMongoRepos is responsible for performing CRUD operations on the "users" collection in MongoDB.
    /// It interacts with MongoDB to manage user entities.
    /// </summary>
    public class UserMongoRepos : IUserRepos
    {
        /// <summary>
        /// The MongoDB collection used to access user data.
        /// </summary>
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Initializes a new instance of the UserMongoRepos class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service used to access the "users" collection.</param>
        public UserMongoRepos(IMongoService mongoService)
        {
            _users = mongoService.GetCollection<User>("users");
        }

        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>The requested user entity or null if not found.</returns>
        public async Task<User> Read(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all users associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution to filter users by.</param>
        /// <returns>A collection of users associated with the specified institution.</returns>
        public async Task<IEnumerable<User>> ReadAll(string institutionId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.InstitutionId, institutionId);
            return await _users.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new user in the database.
        /// If the user already has an ID, it is set to null before insertion.
        /// </summary>
        /// <param name="entity">The user entity to be created.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        public async Task Create(User entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _users.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing user identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="entity">The updated user entity.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> Update(string id, User entity)
        {
            entity.Id = id;

            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var updateDefinitions = new List<UpdateDefinition<User>>();

            if (entity.Username != null)
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Username, entity.Username));
            if (entity.Password != null)
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Password, entity.Password));

            updateDefinitions.Add(Builders<User>.Update.Set(u => u.Name, entity.Name));
            updateDefinitions.Add(Builders<User>.Update.Set(u => u.Email, entity.Email));
            updateDefinitions.Add(Builders<User>.Update.Set(u => u.Phone, entity.Phone));
            updateDefinitions.Add(Builders<User>.Update.Set(u => u.Role, entity.Role));
            updateDefinitions.Add(Builders<User>.Update.Set(u => u.InstitutionId, entity.InstitutionId));

            var update = Builders<User>.Update.Combine(updateDefinitions);
            var updateResult = await _users.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a user from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A boolean indicating whether the delete operation was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await _users.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves a user based on their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The requested user entity or null if not found.</returns>
        public async Task<User> ReadOnUsername(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }
    }
}