namespace Book.Application.Domain.Repository
{
    public interface IBookRepository
    {
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(int id, Book book);
        Task SetAuthorsAsync(int id, IList<int> authors);
        Task<Book> DeleteAsync(int id);
        Task<Book> GetByIdAsync(int id);
        Task<Paged<Book>> GetAsync(Filter filter);
    }
}
