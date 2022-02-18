using Book.Application.Domain;
using Book.Application.Domain.Helper;
using Book.Application.Domain.Repository;
using Book.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Book.Application.Infrastructure.Sql.Repository
{
    public class SqlAuthorRepository : IAuthorRepository
    {
        private readonly Context _context;
        private readonly IQueryHelper<Author> _queryHelper;
        public SqlAuthorRepository(Context context, IQueryHelper<Author> queryHelper)
        {
            _context = context;
            _queryHelper = queryHelper;
        }
        public async Task<Author> AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            return author;
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(c => c.Id == id)??throw new AuthorNotFoundException();
            author.Delete();
        }

        public async Task<Paged<Author>> GetAsync(Filter filter)
        {

            var query = _context.Authors.Where(c=>!c.IsDeleted).AsQueryable();

            query = _queryHelper.ApplySearch(query, filter.SearchValue, filter.SearchFields);

            query = _queryHelper.ApplySort(query, filter.OrderBy);

            return await _queryHelper.ApplyPaginationAsync(query, filter.CurrentPage, filter.PageSize);
        }

        public async Task<Author> UpdateNameAsync(int id, string name)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(c => c.Id == id) ?? throw new AuthorNotFoundException();
            author.SetName(name);
            return author;
        }
    }
}
