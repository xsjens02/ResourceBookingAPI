using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class InstitutionMongoRepos : IInstitutionRepos<Institution, string>
    {
        private IMongoCollection<Institution> _institutions;
        public InstitutionMongoRepos(IMongoService mongoService)
        {
            _institutions = mongoService.GetCollection<Institution>("institutions");
        }
        public async Task<Institution> Get(string id)
        {
            return await _institutions.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(string id, [FromBody] Institution entity)
        {
            entity.Id = id;

            var filter = Builders<Institution>.Filter.Eq(i => i.Id, id);
            var result = await _institutions.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }
    }
}
