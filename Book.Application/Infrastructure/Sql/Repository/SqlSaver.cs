using Book.Application.Domain.Repository;

namespace Book.Application.Infrastructure.Sql.Repository
{
    public class SqlSaver : ISaver
    {
        private readonly Context _context;

        public SqlSaver(Context context)
        {
            _context = context;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
