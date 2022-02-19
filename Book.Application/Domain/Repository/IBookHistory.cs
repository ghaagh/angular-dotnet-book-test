namespace Book.Application.Domain.Repository
{
    public interface IHistory<T> where T : IAggregateRoot
    {
        Task<IList<History>> GetAsync(int id);
    }
}
