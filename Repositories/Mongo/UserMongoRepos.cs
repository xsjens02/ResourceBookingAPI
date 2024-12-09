using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// Repository for managing User entities in a MongoDB database.
    /// Implements basic CRUD operations for handling users and provides password validation.
    /// </summary>
    public class UserMongoRepos : ICrudRepos<User, string>, ILoginRepos
    {
        private readonly IMongoCollection<User> _users;
        private readonly PasswordHasher<User> _passwordHasher;

        /// <summary>
        /// Initializes the User repository with a MongoDB collection and a password hasher.
        /// </summary>
        /// <param name="mongoService">Service for accessing MongoDB.</param>
        public UserMongoRepos(IMongoService mongoService)
        {
            _users = mongoService.GetCollection<User>("users");
            _passwordHasher = new PasswordHasher<User>();
        }

        /// <summary>
        /// Retrieves a single user by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The matching User, with sensitive data (Username, Password) removed.</returns>
        public async Task<User> Get(string id)
        {
            var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Username = null;
                user.Password = null;
            }

            return user;
        }

        /// <summary>
        /// Retrieves all users for a given institution ID.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to filter users by.</param>
        /// <returns>A list of users belonging to the specified institution, with sensitive data removed.</returns>
        public async Task<IEnumerable<User>> GetAll([FromQuery] string institutionId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.InstitutionId, institutionId);
            var users = await _users.Find(filter).ToListAsync();

            foreach (var user in users)
            {
                user.Username = null;
                user.Password = null;
            }

            return users;
        }

        /// <summary>
        /// Creates a new user in the database, hashing their password before storing.
        /// </summary>
        /// <param name="entity">The User entity to be created.</param>
        public async Task Create([FromBody] User entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            await _users.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing user's information by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="entity">The updated User entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> Update(string id, [FromBody] User entity)
        {
            entity.Id = id;

            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var updateDefinitions = new List<UpdateDefinition<User>>();

            if (entity.Username != null)
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Username, entity.Username));
            if (entity.Password != null)
            {
                entity.Password = _passwordHasher.HashPassword(entity, entity.Password);
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Password, entity.Password));
            }

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
        /// Deletes a user by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await _users.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Fetches a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The matching User or null if not found.</returns>
        public async Task<User> FetchUser(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Validates the password for a given user.
        /// </summary>
        /// <param name="user">The user whose password needs validation.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password is correct; otherwise, false.</returns>
        public bool ValidatePassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}