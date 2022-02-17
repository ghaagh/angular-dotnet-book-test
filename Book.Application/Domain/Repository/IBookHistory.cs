namespace Book.Application.Domain.Repository
{
    public interface IBookHistory
    {
        Task<IList<History>> GetAsync();
    }
}
