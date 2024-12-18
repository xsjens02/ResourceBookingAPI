using ResourceBookingAPI.Interfaces.Managers;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Managers
{
    /// <summary>
    /// BookingManager is responsible for managing booking-related operations.
    /// It interacts with the booking repository to perform CRUD operations
    /// and additional business logic related to bookings.
    /// </summary>
    public class BookingManager : IBookingManager
    {
        /// <summary>
        /// The repository instance used to access booking data.
        /// </summary>
        private readonly IBookingRepos _bookingRepo;

        /// <summary>
        /// Initializes a new instance of the BookingManager class.
        /// </summary>
        /// <param name="_bookingRepo">The booking repository to be used by this manager.</param>
        public BookingManager(IBookingRepos _bookingRepo)
        {
            this._bookingRepo = _bookingRepo;
        }

        /// <summary>
        /// Clears all upcoming bookings for a specified institution starting from the current date.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution whose bookings should be cleared.</param>
        /// <returns>A task representing the asynchronous operation, returning true if bookings were successfully cleared, false otherwise.</returns>
        public async Task<bool> ClearUpcomingInstitutionBookings(string institutionId)
        {
            var currentDate = DateTime.UtcNow;
            return await _bookingRepo.DeleteOnInstitutionByDate(institutionId, currentDate);
        }


        /// <summary>
        /// Clears all upcoming bookings for a specified resource starting from the current date.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource whose bookings should be cleared.</param>
        /// <returns>A task representing the asynchronous operation, returning true if bookings were successfully cleared, false otherwise.</returns>
        public async Task<bool> ClearUpcomingResourceBookings(string resourceId)
        {
            var currentDate = DateTime.UtcNow;
            return await _bookingRepo.DeleteOnResourceByDate(resourceId, currentDate);
        }

        /// <summary>
        /// Creates a new booking in the system.
        /// </summary>
        /// <param name="entity">The booking entity to create.</param>
        public async Task Create(Booking entity)
        {
            await _bookingRepo.Create(entity);
        }

        /// <summary>
        /// Deletes a booking by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to delete.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Delete(string id)
        {
            return await _bookingRepo.Delete(id);
        }

        /// <summary>
        /// Retrieves a booking by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to retrieve.</param>
        /// <returns>The requested booking entity.</returns>
        public async Task<Booking> Get(string id)
        {
            return await _bookingRepo.Read(id);
        }

        /// <summary>
        /// Retrieves all bookings associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of bookings associated with the user.</returns>
        public async Task<IEnumerable<Booking>> GetAll(string userId)
        {
            return await _bookingRepo.ReadAll(userId);
        }

        /// <summary>
        /// Retrieves pending bookings for a user on or after a specific date.
        /// The results are sorted by date and start time.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="currentDate">The current date used as the filter threshold.</param>
        /// <returns>A collection of pending bookings for the user.</returns>
        public async Task<IEnumerable<Booking>> GetPendingUserBookings(string userId, DateTime currentDate)
        {
            var bookings = await _bookingRepo.GetOnUserByDate(userId, currentDate);
            var sortedBookings = bookings
                .OrderBy(b => b.Date)
                .ThenBy(b => TimeSpan.Parse(b.StartTime!))
                .ToList();
            return sortedBookings;
        }

        /// <summary>
        /// Retrieves bookings for a specific resource on a given date.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource.</param>
        /// <param name="searchDate">The date for which bookings should be retrieved.</param>
        /// <returns>A collection of bookings for the specified resource on the given date.</returns>
        public async Task<IEnumerable<Booking>> GetResourceBookingsOnDate(string resourceId, DateTime searchDate)
        {
            return await _bookingRepo.GetOnResourceByDate(resourceId, searchDate);
        }

        /// <summary>
        /// Retrieves booking statistics for a specific institution within a date range.
        /// </summary>
        /// <param name="institutionId">The unique identifier of the institution.</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of bookings within the specified date range.</returns>
        public async Task<IEnumerable<Booking>> GetStatistics(string institutionId, DateTime startDate, DateTime endDate)
        {
            return await _bookingRepo.GetOnInstitutionByDateRange(institutionId, startDate, endDate);
        }

        /// <summary>
        /// Updates an existing booking identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to update.</param>
        /// <param name="entity">The updated booking entity.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Update(string id, Booking entity)
        {
            return await _bookingRepo.Update(id, entity);
        }
    }
}
