using MongoDB.Driver;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Repositories.Mongo
{
    /// <summary>
    /// BookingMongoRepos is responsible for interacting with the MongoDB database
    /// to perform CRUD operations for booking entities. It utilizes the MongoDB
    /// driver to access the "bookings" collection and manage booking data.
    /// </summary>
    public class BookingMongoRepos : IBookingRepos
    {
        /// <summary>
        /// The MongoDB collection used to access booking data.
        /// </summary>
        private IMongoCollection<Booking> _bookings;

        /// <summary>
        /// Initializes a new instance of the BookingMongoRepos class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service to provide access to the "bookings" collection.</param>
        public BookingMongoRepos(IMongoService mongoService)
        {
            _bookings = mongoService.GetCollection<Booking>("bookings");
        }

        /// <summary>
        /// Retrieves a booking by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to retrieve.</param>
        /// <returns>The requested booking entity or null if not found.</returns>
        public async Task<Booking> Read(string id)
        {
            return await _bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all bookings associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose bookings to retrieve.</param>
        /// <returns>A collection of bookings associated with the user.</returns>
        public async Task<IEnumerable<Booking>> ReadAll(string userId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.UserId, userId);
            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Retrieves all bookings associated with a specific institution within a date range.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution whose bookings to retrieve.</param>
        /// <param name="startDate">The start date of the date range.</param>
        /// <param name="endDate">The end date of the date range.</param>
        /// <returns>A collection of bookings associated with the institution within the date range.</returns>
        public async Task<IEnumerable<Booking>> GetOnInstitutionByDateRange(string institutionId, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.InstitutionId, institutionId),
                Builders<Booking>.Filter.Gte(b => b.Date, startDate),
                Builders<Booking>.Filter.Lte(b => b.Date, endDate)
             );

            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Retrieves all bookings for a specific user on or after a given date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose bookings to retrieve.</param>
        /// <param name="currentDate">The current date for filtering bookings.</param>
        /// <returns>A collection of bookings for the user by date.</returns>
        public async Task<IEnumerable<Booking>> GetOnUserByDate(string userId, DateTime currentDate)
        {
            currentDate = currentDate.Date; 
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.UserId, userId),
                Builders<Booking>.Filter.Gte(b => b.Date, currentDate)
            );

            return await _bookings.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Retrieves all bookings for a specific resource on a given date.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource whose bookings to retrieve.</param>
        /// <param name="searchDate">The date for filtering bookings.</param>
        /// <returns>A collection of bookings for the resource on the specified date.</returns>
        public async Task<IEnumerable<Booking>> GetOnResourceByDate(string resourceId, DateTime searchDate)
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
        /// If the booking already has an ID, it is set to null before insertion.
        /// </summary>
        /// <param name="entity">The booking entity to be created.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        public async Task Create(Booking entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = null;

            await _bookings.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates an existing booking identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to update.</param>
        /// <param name="entity">The updated booking entity.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> Update(string id, Booking entity)
        {
            entity.Id = id;

            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a booking by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var result = await _bookings.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Deletes all bookings for a specific resource from the specified date onwards.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource.</param>
        /// <param name="date">The start date from which bookings will be deleted.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteOnResourceByDate(string resourceId, DateTime date)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.ResourceId, resourceId),
                Builders<Booking>.Filter.Gte(b => b.Date, date)
            );

            var result = await _bookings.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Deletes all bookings for a specific institution from the specified date onwards.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution.</param>
        /// <param name="date">The start date from which bookings will be deleted.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteOnInstitutionByDate(string institutionId, DateTime date)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.InstitutionId, institutionId),
                Builders<Booking>.Filter.Gte(b => b.Date, date)
            );

            var result = await _bookings.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}