
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Book.Application.Domain.ChangeHistory;

/// <summary>
/// This class streams/gets the changes of data for insertion or modification on books.
/// </summary>
public class ContextChangeHandler : IContextChangeHandler
{
    public event EventHandler<OnChangedEventArgument> HistoryChanged;
    private readonly Context _context;
    private readonly List<Change> _changes;
    public ContextChangeHandler(Context context)
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
            //It is not a modification so return;
            if (entity.Id <= 0)
                continue;
            foreach (var member in item.Members.Where(c => c.IsModified))
            {

                switch (member)
                {
                    case PropertyEntry propertyEntry:
                        _changes.Add(ExtractChange(entity.Id, propertyEntry));
                        break;
                    case CollectionEntry collectionEntry:
                        var deletedObjects = _context.ChangeTracker.Entries()
                                                .Where(c => c.State == EntityState.Deleted && c.Entity.GetType() == typeof(AuthorBook) && (c.Entity as AuthorBook).BookId == entity.Id)
                                                .Select(c => c.Entity as AuthorBook).ToList();
                        var change = ExtractChange(entity.Id, collectionEntry, deletedObjects);
                        savedAthorOnBooks = true;
                        if (change == null)
                            continue;
                        _changes.Add(change);

                        break;
                }
            }
        }
        //it is already capturedNo Need to look at authors book context. 
        if (savedAthorOnBooks)
        {
            return;
        }

        var authorChanges = _context.ChangeTracker.Entries()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) &&
        e.Entity.GetType() == typeof(AuthorBook)).Select(c => c.Entity as AuthorBook).GroupBy(c => c.BookId);
        var addedAuthors = new List<AuthorBook>();

        foreach (var groupedItem in authorChanges)
        {
            var bookId = groupedItem.Key;
            //it is not a modification. so return.
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


            _changes.Add(new Change
            {
                Id = bookId,
                Field = "AuthorBooks",
                OldValue = deletedAuthors,
                NewValue = addedAuthors
            });
        }
    }

    private static Change ExtractChange(int id, PropertyEntry propertyEntry)
    {
        var currentValue = propertyEntry.CurrentValue?.ToString();
        var originalValue = propertyEntry.OriginalValue?.ToString();
        return new Change
        {
            Id = id,
            Field = propertyEntry.Metadata.Name,
            NewValue = currentValue,
            OldValue = originalValue
        };

    }

    private static Change ExtractChange(int id, CollectionEntry collectionEntry, object deletedEntries)
    {
        var newAuthors = collectionEntry.CurrentValue as List<AuthorBook>;
        var deletedAuthors = deletedEntries as List<AuthorBook>;

        if (deletedAuthors.SequenceEqual(newAuthors))
            return null;

        return new Change
        {
            Id = id,
            Field = collectionEntry.Metadata.Name,
            NewValue = collectionEntry.CurrentValue,
            OldValue = deletedAuthors
        };

    }

    private void Invoke()
    {
        HistoryChanged.Invoke(this, new OnChangedEventArgument() { Changes = _changes });
        _changes.Clear();
    }
}


