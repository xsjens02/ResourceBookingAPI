namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface IReadAllRepos<T, TKey>
    {
        Task<IEnumerable<T>> ReadAll(TKey id);
    }
}
