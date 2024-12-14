using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IResourceRepos :
        ICrudRepos<Resource, string>
    {
    }
}
