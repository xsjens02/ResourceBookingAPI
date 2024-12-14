namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface ICrudRepos<T, TKey> :
        IReadRepos<T, TKey>,
        IReadAllRepos<T, TKey>,
        ICreateRepos<T>,
        IUpdateRepos<T, TKey>,
        IDeleteRepos<TKey>
    {
    }
}
