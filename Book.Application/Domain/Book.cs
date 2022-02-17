namespace Book.Application.Domain
{
    public class Book : IAggregateRoot
    {
        private Book() { }
        public Book(string bookTitle, string isbn, DateTime publishedAt, IList<int> authorIds)
        {
            BookTitle = bookTitle;
            ISBN = isbn;
            PublishedAt = publishedAt;
            SetAuthors(authorIds);
        }

        public int Id { get; set; }
        public string? BookTitle { get; set; }
        public string? ISBN { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsDeleted { get; set; }
        public virtual IEnumerable<AuthorBook> AuthorBooks { get; set; }
        public void SetAuthors(IList<int> authorIds)
        {
            AuthorBooks = authorIds.Select(c => new AuthorBook(Id, c));
        }
        public void Delete() => IsDeleted = true;
    }
}
