using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IBookingRepos :
        ICrudRepos<Booking, string>
    {
        Task<IEnumerable<Booking>> GetOnInstitutionByDateRange(string institutionId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetOnUserByDate(string userId, DateTime currentDate);
        Task<IEnumerable<Booking>> GetOnResourceByDate(string resourceId, DateTime searchDate);
    }
}
