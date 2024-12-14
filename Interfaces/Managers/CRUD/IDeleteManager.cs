namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface IDeleteManager<TKey>
    {
        Task<bool> Delete(TKey id);
    }
}
