namespace Book.Application.Domain
{
    public class Book : IAggregateRoot
    {
        private Book() { }

        public Book(string bookTitle, string isbn, DateTime publishedAt, IList<int> authorIds,string? description= null)
        {
            BookTitle = bookTitle;
            ISBN = isbn;
            PublishedAt = publishedAt;
            Description = description;
            SetAuthors(authorIds);
        }

        public int Id { get; set; }
        public string? BookTitle { get; set; }
        public string? ISBN { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
        public void SetAuthors(IList<int> authorIds)
        {
            AuthorBooks = new List<AuthorBook>();
            AuthorBooks = authorIds.Select(c => new AuthorBook(Id, c)).ToList();
        }
        public void Delete() => IsDeleted = true;
    }
}
