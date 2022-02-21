using Book.Application.Domain;
using Book.Application.Domain.ChangeHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Book.Application.Infrastructure.Sql.ChangeHistory;

/// <summary>
/// This class streams/gets the changes of data for insertion or modification on books.
/// </summary>
public class SqlContextChangeHandler : IContextChangeHandler
{
    public event EventHandler<OnChangedEventArgument> HistoryChanged;
    private readonly Context _context;
    private readonly List<Change> _changes;
    public SqlContextChangeHandler(Context context)
    {
        _changes = new List<Change>();
        _context = context;
        
    }

    /// <summary>
    /// OneSaveFinished. it is called by context when saveChanges happened.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    public void OnSaveFinished(object sender, EntityStateChangedEventArgs ev)
    {

        if (ev.OldState != EntityState.Added || ev.NewState != EntityState.Unchanged)
        {
            Invoke();
            return;
        }

        if (ev.Entry.Metadata.Name != "Book.Application.Domain.Book")
        {
            Invoke();
            return;
        }
        if (ev.Entry.Entity is Domain.Book bookEntity)
        {
            foreach (var item in bookEntity.GetType().GetProperties())
            {
                _changes.Add(new Change()
                {
                    Id = bookEntity.Id,
                    Field = item.Name,
                    NewValue = item.GetValue(bookEntity),
                });
            }
        }
        Invoke();
    }

    /// <summary>
    /// StartTracking. This method is for tracking modification not insertion of context books.
    /// </summary>
    public void OnSaving(object sender, SavingChangesEventArgs ev)
    {
        var savedAthorOnBooks = false;
        var bookChanges = _context.ChangeTracker.Entries().Where(e =>
        (e.State == EntityState.Added || e.State == EntityState.Modified)
        && e.Entity.GetType() == typeof(Domain.Book));
        foreach (var item in bookChanges)
        {

            var entity = item.Entity as Domain.Book;
            //It is not a modification.
            if (entity.Id <= 0)
                continue;
            foreach (var member in item.Members.Where(c => c.IsModified))
            {

                if (member is PropertyEntry propertyMember)
                {
                    var currentValue = member.CurrentValue?.ToString();
                    var originalValue = propertyMember.OriginalValue?.ToString();

                    _changes.Add(new Change
                    {
                        Id = entity.Id,
                        Field = member.Metadata.Name,
                        NewValue = currentValue,
                        OldValue = originalValue
                    });
                }
                else if (member is CollectionEntry collectionEntry)
                {

                    var deletedAuthors = _context.ChangeTracker.Entries()
                    .Where(c => c.State == EntityState.Deleted &&
                    c.Entity.GetType() == typeof(AuthorBook) &&
                    (c.Entity as AuthorBook).BookId == entity.Id)
                        .Select(c => c.Entity as AuthorBook).ToList();


                    _changes.Add(new Change
                    {
                        Id = entity.Id,
                        Field = member.Metadata.Name,
                        NewValue = member.CurrentValue,
                        OldValue = deletedAuthors
                    });
                    savedAthorOnBooks = true;
                }
            }
        }

        if (savedAthorOnBooks)
        {
            return;
        }

        var authorChanges = _context.ChangeTracker.Entries().Where(e =>
        (e.State == EntityState.Added || e.State == EntityState.Modified) &&
        e.Entity.GetType() == typeof(Domain.AuthorBook)).Select(c => c.Entity as AuthorBook).GroupBy(c => c.BookId);
        var addedAuthors = new List<AuthorBook>();

        foreach (var groupedItem in authorChanges)
        {
            var bookId = groupedItem.Key;
            //it is not a modification
            if (bookId <= 0)
                continue;
            foreach (var item in groupedItem)
            {
                var authorId = item.AuthorId;
                addedAuthors.Add(new AuthorBook(bookId, authorId));
            }
            var deletedAuthors = _context.ChangeTracker.Entries().Where(c =>
            c.State == EntityState.Deleted
            && c.Entity.GetType() == typeof(AuthorBook)
            && (c.Entity as AuthorBook).BookId == bookId)
                .Select(c => c.Entity as AuthorBook).ToList();


            _changes.Add(new Change()
            {
                Id = bookId,
                Field = "AuthorBooks",
                OldValue = deletedAuthors,
                NewValue = addedAuthors
            });
        }
    }
    private void Invoke()
    {
        HistoryChanged.Invoke(this, new OnChangedEventArgument() { Changes = _changes });
        _changes.Clear();
    }
}


