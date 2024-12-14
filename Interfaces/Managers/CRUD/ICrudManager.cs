namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface ICrudManager<T, TKey> :
        IGetManager<T, TKey>,
        IGetAllManager<T, TKey>,
        ICreateManager<T>,
        IUpdateManager<T, TKey>,
        IDeleteManager<TKey>
    {
    }
}
