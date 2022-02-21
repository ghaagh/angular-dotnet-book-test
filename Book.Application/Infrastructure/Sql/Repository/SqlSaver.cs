using Book.Application.Domain;
using Book.Application.Domain.ChangeHistory;
using Book.Application.Domain.Repository;

namespace Book.Application.Infrastructure.Sql.Repository;
public class SqlSaver : ISaver
{
    private readonly Context _context;
    private readonly IContextChangeHandler _contextChangeHandler;
    /// <summary>
    /// Context Change Handler and History Handler should be injected at least once in code!
    /// </summary>
    /// <param name="context"></param>
    /// <param name="contextChangeHandler"></param>
    /// <param name="historyHandler"></param>
    public SqlSaver(Context context, IContextChangeHandler contextChangeHandler, IHistoryHandler historyHandler)
    {
        _context = context;
        _contextChangeHandler = contextChangeHandler;
    }

    public async Task<int> SaveAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result;
    }
}


