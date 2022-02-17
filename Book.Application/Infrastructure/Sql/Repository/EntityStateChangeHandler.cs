using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Book.Application.Infrastructure.Sql.Repository
{
    public class EntityStateChangeHandler
    {
        public void StateChanged(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.GetType() != typeof(Book.Application.Domain.Book))
                return;
        }
    }
}
