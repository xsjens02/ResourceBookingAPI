using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.Interfaces.Services;

namespace ResourceBookingAPI.Services
{
    /// <summary>
    /// Service for interacting with MongoDB database.
    /// Provides methods to access and interact with MongoDB collections.
    /// </summary>
    public class MongoService : IMongoService
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Initializes the MongoService with MongoDB configuration and establishes a database connection.
        /// </summary>
        /// <param name="mongoDb">The configuration containing the MongoDB connection string and database name.</param>
        public MongoService(IOptions<MongoConfig> mongoDb)
        {
            var client = new MongoClient(mongoDb.Value.ConnectionString);
            _database = client.GetDatabase(mongoDb.Value.DatabaseName);
        }

        /// <summary>
        /// Retrieves a MongoDB collection by its name.
        /// </summary>
        /// <typeparam name="T">The type of the collection's documents.</typeparam>
        /// <param name="collectionName">The name of the collection to retrieve.</param>
        /// <returns>An IMongoCollection of the specified type.</returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}