using Book.Application.Domain;
using Book.Application.Domain.Helper;
using Book.Application.Domain.Repository;
using Book.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Book.Application.Infrastructure.Sql.Repository
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly Context _db;
        private readonly IQueryHelper<Domain.Book> _queryHelper;

        public SqlBookRepository(Context context, IQueryHelper<Domain.Book> queryHelper)
        {
            _db = context;
            _queryHelper = queryHelper;
        }

        public async Task<Domain.Book> AddAsync(Domain.Book book)
        {
            await _db.Books.AddAsync(book);
            return book;
        }

        public async Task<Domain.Book> DeleteAsync(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new BookNotFoundException();
            book.Delete();
            return book;
        }

        public async Task<Paged<Domain.Book>> GetAsync(Filter filter)
        {
            var query = _db.Books.AsQueryable();

            query = _queryHelper.ApplySearch(query, filter.SearchValue, filter.SearchFields);

            query = _queryHelper.ApplySort(query, filter.OrderBy);

            return await _queryHelper.ApplyPaginationAsync(query, filter.CurrentPage, filter.PageSize);
        }

        public async Task<Domain.Book> GetByIdAsync(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new BookNotFoundException();
        }

        public async Task<Domain.Book> UpdateAsync(int id, Domain.Book book)
        {
            var currentBook = await _db.Books.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new BookNotFoundException();
            currentBook = book;
            return book;
        }

        public async Task SetAuthorsAsync(int id, IList<int> authorIds)
        {
            var book = await _db.Books.Include(c => c.AuthorBooks).FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new BookNotFoundException();
            book.SetAuthors(authorIds);
        }
    }
}
