using Book.Application.Domain;
using Book.Application.Domain.Helper;
using Book.Application.Domain.Repository;

namespace Book.Application.Infrastructure.Sql.Repository
{
    public class SqlBookHistoryRepository : IBookHistoryRepository
    {
        private readonly Context _db;
        private readonly IQueryHelper<Domain.BookHistory> _queryHelper;
        public SqlBookHistoryRepository(Context context, IQueryHelper<Domain.BookHistory> queryHelper)
        {
            _db = context;
            _queryHelper = queryHelper;
        }
        public async Task<Paged<BookHistory>> GetAsync(int id, Filter filter)
        {
            var query = _db.BookHistories.Where(c => c.BookId == id).AsQueryable();

            query = _queryHelper.ApplySearch(query, filter.SearchValue, filter.SearchFields);

            query = _queryHelper.ApplySort(query, filter.OrderBy);

            return await _queryHelper.ApplyPaginationAsync(query, filter.CurrentPage, filter.PageSize);
        }
    }
}
