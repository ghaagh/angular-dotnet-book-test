namespace Book.Application.Domain.Repository
{
    public interface IBookHistoryRepository
    {
        Task<Paged<BookHistory>> GetAsync(int id, Filter filter);
    }
}
