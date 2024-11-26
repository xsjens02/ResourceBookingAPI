using MongoDB.Driver;

namespace ResourceBookingAPI.Interfaces.Services
{
    public interface IMongoService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
