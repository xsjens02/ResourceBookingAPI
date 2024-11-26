namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface ICrudController<T, TKey> :
        IReadController<TKey>,
        IReadAllController,
        ICreateController<T>,
        IUpdateController<T, TKey>,
        IDeleteController<TKey>
    {
    }
}
