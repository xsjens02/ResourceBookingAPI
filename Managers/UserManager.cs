using Microsoft.AspNetCore.Identity;
using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// UserManager is responsible for managing user-related operations, 
    /// including creating, retrieving, updating, deleting users, 
    /// and validating user passwords. It interacts with the user repository
    /// and utilizes a password hasher to handle password-related functionality.
    /// </summary>
    public class UserManager : IUserManager
    {
        /// <summary>
        /// The repository instance used to access user data.
        /// </summary>
        private readonly IUserRepos _userRepos;

        /// <summary>
        /// The password hasher instance used for hashing and validating passwords.
        /// </summary>
        private readonly PasswordHasher<User> _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the UserManager class.
        /// </summary>
        /// <param name="userRepos">The repository to be used for user operations.</param>
        public UserManager(IUserRepos userRepos)
        {
            _userRepos = userRepos;
            _passwordHasher = new PasswordHasher<User>();
        }

        /// <summary>
        /// Creates a new user in the system and hashes the user's password before saving.
        /// </summary>
        /// <param name="entity">The user entity to be created.</param>
        /// <returns>A task that represents the asynchronous creation operation.</returns>
        public async Task Create(User entity)
        {
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password!);
            await _userRepos.Create(entity);
        }

        /// <summary>
        /// Deletes a user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous deletion operation. 
        /// The task result is a boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            return await _userRepos.Delete(id);
        }

        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>The requested user entity with sensitive data removed (username and password).</returns>
        public async Task<User> Get(string id)
        {
            var user = await _userRepos.Read(id);
            if (user != null)
            {
                user.Username = null; // Remove sensitive information
                user.Password = null; // Remove sensitive information
            }

            return user;
        }

        /// <summary>
        /// Retrieves all users associated with a specific institution.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution whose users to retrieve.</param>
        /// <returns>A collection of users associated with the institution, with sensitive data removed.</returns>
        public async Task<IEnumerable<User>> GetAll(string institutionId)
        {
            var users = await _userRepos.ReadAll(institutionId);
            foreach (var user in users)
            {
                user.Username = null; // Remove sensitive information
                user.Password = null; // Remove sensitive information
            }

            return users;
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The user entity associated with the given username.</returns>
        public async Task<User> GetUserFromUsername(string username)
        {
            return await _userRepos.ReadOnUsername(username);
        }

        /// <summary>
        /// Validates a user's password by comparing the provided password with the hashed password.
        /// </summary>
        /// <param name="user">The user whose password is to be validated.</param>
        /// <param name="password">The plain text password to validate.</param>
        /// <returns>A boolean indicating whether the provided password is valid.</returns>
        public bool ValidatePassword(User user, string password)
        {
            if (user.Password == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }

        /// <summary>
        /// Updates an existing user identified by its unique identifier.
        /// If a new password is provided, it will be hashed before updating the user.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="entity">The updated user entity.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Update(string id, User entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Password))
                entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            return await _userRepos.Update(id, entity);
        }
    }
}