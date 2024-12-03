using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class UserMongoRepos : ICrudRepos<User, string>, ILoginRepos
    {
        private IMongoCollection<User> _users;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserMongoRepos(IMongoService mongoService)
        {
            _users = mongoService.GetCollection<User>("users");
            _passwordHasher = new PasswordHasher<User>();
        }
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

        public async Task Create([FromBody] User entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            await _users.InsertOneAsync(entity);
        }

        public async Task<bool> Update(string id, [FromBody] User entity)
        {
            entity.Id = id;

            var filter = Builders<User>.Filter.Eq(u => u.Id, id);

            if (entity.Password == null)
            {
                var update = Builders<User>.Update
                    .Set(u => u.Name, entity.Name)
                    .Set(u => u.Email, entity.Email)
                    .Set(u => u.Phone, entity.Phone)
                    .Set(u => u.Role, entity.Role)
                    .Set(u => u.InstitutionId, entity.InstitutionId);

                var updateResult = await _users.UpdateOneAsync(filter, update);
                return updateResult.ModifiedCount > 0;
            }

            var replaceResult = await _users.ReplaceOneAsync(filter, entity);
            return replaceResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await _users.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<User> FetchUser(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public bool ValidatePassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
