using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class ResourceMongoRepos : ICrudRepos<Resource, string>
    {
        private IMongoCollection<Resource> _resources;
        public ResourceMongoRepos(IMongoService mongoService)
        {
            _resources = mongoService.GetCollection<Resource>("resources");
        }
        public async Task<Resource> Get(string id)
        {
            return await _resources.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Resource>> GetAll([FromQuery] string institutionId)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.InstitutionId, institutionId);
            return await _resources.Find(filter).ToListAsync();
        }

        public async Task Create([FromBody] Resource entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _resources.InsertOneAsync(entity);
        }

        public async Task<bool> Update(string id, [FromBody] Resource entity)
        {
            entity.Id = id;

            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Resource>.Filter.Eq(r => r.Id, id);
            var result = await _resources.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
