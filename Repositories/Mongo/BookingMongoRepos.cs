﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    public class BookingMongoRepos : IBookingRepos
    {
        private IMongoCollection<Booking> _bookings;
        public BookingMongoRepos(IMongoService mongoService)
        {
            _bookings = mongoService.GetCollection<Booking>("bookings");
        }

        public async Task<Booking> Get(string id)
        {
            return await _bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetAll([FromQuery] string userId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.UserId, userId);
            return await _bookings.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllWithinDates(DateTime startDate, DateTime endDate, string institutionId)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.InstitutionId, institutionId),  
                Builders<Booking>.Filter.Gte(b => b.Date, startDate),               
                Builders<Booking>.Filter.Lte(b => b.Date, endDate)                   
             );

            return await _bookings.Find(filter).ToListAsync();
        }

        public async Task Create([FromBody] Booking entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _bookings.InsertOneAsync(entity);
        }

        public async Task<bool> Update(string id, [FromBody] Booking entity)
        {
            entity.Id = id;

            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
