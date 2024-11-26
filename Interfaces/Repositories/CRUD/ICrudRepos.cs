namespace ResourceBookingAPI.Interfaces.Repositories.CRUD
{
    public interface ICrudRepos<T, TKey> :
        IReadRepos<T, TKey>,
        IReadAllRepos<T>,
        ICreateRepos<T>,
        IUpdateRepos<T, TKey>,
        IDeleteRepos<TKey>
    {
    }
}
