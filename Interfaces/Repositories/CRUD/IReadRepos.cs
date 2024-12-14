namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IReadRepos<T, TKey>
    {
        Task<T> Read(TKey id);
    }
}
