using ResourceBookingAPI.Interfaces.Controllers.CRUD;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IInstitutionController<T, TKey> :
        IReadController<TKey>,
        IUpdateController<T, TKey>
    {
    }
}
