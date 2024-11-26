using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Services
{
    public class MongoService : IMongoService
    {
        private readonly IMongoDatabase _database;
        public MongoService(IOptions<MongoConfig> mongoDb)
        {
            var client = new MongoClient(mongoDb.Value.ConnectionString);
            _database = client.GetDatabase(mongoDb.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
