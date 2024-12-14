namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface IUpdateManager<T, TKey>
    {
        Task<bool> Update(TKey id, T entity);
    }
}
