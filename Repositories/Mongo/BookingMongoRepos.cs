using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// Repository for managing Booking entities in a MongoDB database.
    /// Provides CRUD operations and specific queries for bookings.
    /// </summary>
    public class BookingMongoRepos : IBookingRepos
    {
        private IMongoCollection<Booking> _bookings;

        /// <summary>
        /// Initializes the Booking repository with a MongoDB collection.
        /// </summary>
        public BookingMongoRepos(IMongoService mongoService)
        {
            _bookings = mongoService.GetCollection<Booking>("bookings");
        }

        /// <summary>
        /// Retrieves a single booking by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>The matching Booking or null if not found.</returns>
        public async Task<Booking> Get(string id)
        {
            return await _bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose bookings to be retrieved.</param>
        /// <returns>A list of bookings for the specified user.</returns>
        public async Task<IEnumerable<Booking>> GetAll([FromQuery] string userId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.UserId, userId);
            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Retrieves all bookings within a specified date range and institution.
        /// </summary>
        /// <param name="startDate">Start date of the range.</param>
        /// <param name="endDate">End date of the range.</param>
        /// <param name="institutionId">ID of the institution.</param>
        /// <returns>A list of bookings that match the criteria.</returns>
        public async Task<IEnumerable<Booking>> GetAllWithinDates(DateTime startDate, DateTime endDate, string institutionId)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.InstitutionId, institutionId),  
                Builders<Booking>.Filter.Gte(b => b.Date, startDate),               
                Builders<Booking>.Filter.Lte(b => b.Date, endDate)                   
             );

            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Retrieves all pending bookings for a user from the current date onward.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="currentDate">The current date to filter bookings.</param>
        /// <returns>A list of pending bookings for the user.</returns>
        public async Task<IEnumerable<Booking>> GetAllPending(string userId, DateTime currentDate)
        {
            currentDate = currentDate.Date; 
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.UserId, userId),
                Builders<Booking>.Filter.Gte(b => b.Date, currentDate)
            );

            var bookings = await _bookings.Find(filter).ToListAsync();

            var sortedBookings = bookings
                .OrderBy(b => b.Date)
                .ThenBy(b => TimeSpan.Parse(b.StartTime!)) 
                .ToList();

            return sortedBookings;
        }

        /// <summary>
        /// Retrieves all bookings for a specific resource on a given date.
        /// </summary>
        /// <param name="resourceId">The ID of the resource.</param>
        /// <param name="searchDate">The date to search for bookings.</param>
        /// <returns>A list of bookings for the resource on the specified date.</returns>
        public async Task<IEnumerable<Booking>> GetAllResourceBookings(string resourceId, DateTime searchDate)
        {
            var targetDate = searchDate.Date;

            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.ResourceId, resourceId),
                Builders<Booking>.Filter.Eq(b => b.Date, targetDate)
            );

            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Creates a new booking in the database.
        /// </summary>
        /// <param name="entity">The booking to be created.</param>
        public async Task Create([FromBody] Booking entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _bookings.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="entity">The updated booking entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> Update(string id, [FromBody] Booking entity)
        {
            entity.Id = id;

            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
