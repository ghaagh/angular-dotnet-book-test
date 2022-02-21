namespace Book.Application.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() : base("the boook is not found") { }
    }
}
