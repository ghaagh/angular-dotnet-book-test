namespace Book.Application.Domain.Repository
{
    public interface IAuthorRepository
    {
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateNameAsync(int id, string name);
        Task DeleteAsync(int id);
        Task<Paged<Author>> GetAsync(Filter filter);

    }
}
