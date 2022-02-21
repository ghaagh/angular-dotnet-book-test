namespace Book.Application.Domain
{
    public class AuthorBook
    {
        private AuthorBook() { }
        public AuthorBook(int bookId, int authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }
        public int BookId { get; private set; }
        public int AuthorId { get; private set; }
        public virtual Author Author { get; private set; }
        public virtual Book Book { get; private set; }
        public override string ToString()
        {
            return $"{{\"BookID\":\"{BookId}\",\"AuthorId\":\"{AuthorId}\"}}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is AuthorBook authorBookObj)
            {
                return BookId == authorBookObj.BookId && AuthorId == authorBookObj.AuthorId;
            }
            else
                return false;
        }
    }
}
