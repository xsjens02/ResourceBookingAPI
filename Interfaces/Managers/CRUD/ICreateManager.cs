namespace ResourceBookingAPI.Interfaces.Managers.CRUD
{
    public interface ICreateManager<T>
    {
        Task Create(T entity);
    }
}
