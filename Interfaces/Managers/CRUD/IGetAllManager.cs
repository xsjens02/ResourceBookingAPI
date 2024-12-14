namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface IGetAllManager<T, TKey>
    {
        Task<IEnumerable<T>> GetAll(TKey id);
    }
}
