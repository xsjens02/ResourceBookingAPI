using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class LoginMongoRepos : ILoginRepos
    {
        private readonly IMongoCollection<User> _users;
        public LoginMongoRepos(IMongoService mongoService)
        {
            _users = mongoService.GetCollection<User>("users");
        }
        public async Task<User> GetUser(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public Task<bool> ValidatePassword(User user, string password)
        {
            return Task.FromResult(user.Password == password);
        }
    }
}
