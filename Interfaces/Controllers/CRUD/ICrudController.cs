namespace ResourceBookingAPI.Interfaces.Controllers.CRUD
{
    public interface ICrudController<T, TKey> :
        IGetController<TKey>,
        IGetAllController,
        IPostController<T>,
        IPutController<T, TKey>,
        IDeleteController<TKey>
    {
    }
}
