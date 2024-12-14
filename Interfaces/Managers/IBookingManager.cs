using ResourceBookingAPI.Interfaces.Managers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Managers
{
    public interface IBookingManager :
        ICrudManager<Booking, string>
    {
        Task<IEnumerable<Booking>> GetStatistics(string institutionId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetPendingUserBookings(string userId, DateTime currentDate);
        Task<IEnumerable<Booking>> GetResourceBookingsOnDate(string resourceId, DateTime searchDate);
    }
}
