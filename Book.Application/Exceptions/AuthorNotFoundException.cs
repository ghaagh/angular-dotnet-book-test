namespace Book.Application.Exceptions
{
    public class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException() : base("Author is not found")
        {

        }
    }
}
