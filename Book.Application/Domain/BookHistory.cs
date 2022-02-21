namespace Book.Application.Domain
{
    public sealed class BookHistory : History
    {
        private BookHistory() { }
        public BookHistory(int bookId, string field, object oldValue, object currentValue) : base(field, oldValue, currentValue)
        {
            BookId = bookId;
            
            if (currentValue is ICollection<AuthorBook> authorChangeRecords)
            {
                CurrentValue = string.Join(',', authorChangeRecords.Select(c => c.AuthorId));
                if (oldValue==null)
                {
                    Description = $"Added Authors with ids: \"{string.Join(',', authorChangeRecords.Select(c => c.AuthorId))}\" to book record with Id of {BookId} at \"{LogDate:yyyy-MM-dd hh:mm:ss}\"";
                }
                else
                {
                    var authorOldRecord = oldValue as ICollection<AuthorBook>;
                    OldValue = string.Join(',', authorOldRecord.Select(c => c.AuthorId));
                    Description = $"Removed Authors with ids:{string.Join(',', authorOldRecord.Select(c => c.AuthorId))} and Added Authors with ids: \"{string.Join(',', authorChangeRecords.Select(c => c.AuthorId))}\" to book record with Id of {BookId} at \"{LogDate:yyyy-MM-dd hh:mm:ss}\"";
                }
            }
            else
            {
                if (OldValue == null)
                    Description =  $"Added \"{Field}\" with value: \"{CurrentValue}\" to book record with Id of {BookId} at \"{LogDate:yyyy-MM-dd hh:mm:ss}\" ";
                else
                    Description =  $"Modified the value of \"{Field}\" from \"{OldValue}\" to: \"{CurrentValue}\" in book record with Id of {BookId} at \"{LogDate:yyyy-MM-dd hh:mm:ss}\" ";
            }
        }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
