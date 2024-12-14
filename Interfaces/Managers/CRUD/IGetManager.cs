namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface IGetManager<T, TKey>
    {
        Task<T> Get(TKey id);
    }
}
