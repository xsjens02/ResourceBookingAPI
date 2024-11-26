using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class ErrorReportMongoRepos : ICrudRepos<ErrorReport, string>
    {
        private IMongoCollection<ErrorReport> _errorReports;
        public ErrorReportMongoRepos(IMongoService mongoService)
        {
            _errorReports = mongoService.GetCollection<ErrorReport>("errorreports");
        }
        public async Task<ErrorReport> Get(string id)
        {
            return await _errorReports.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ErrorReport>> GetAll([FromQuery] string resourceId)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.ResourceId, resourceId);
            return await _errorReports.Find(filter).ToListAsync();
        }

        public async Task Create([FromBody] ErrorReport entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _errorReports.InsertOneAsync(entity);
        }

        public async Task<bool> Update(string id, [FromBody] ErrorReport entity)
        {
            entity.Id = id;

            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<ErrorReport>.Filter.Eq(e => e.Id, id);
            var result = await _errorReports.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
