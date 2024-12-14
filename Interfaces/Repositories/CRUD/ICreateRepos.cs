namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface ICreateRepos<T>
    {
        Task Create(T entity);
    }
}
