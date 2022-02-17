namespace Book.Application.Controllers.Dto
{
    public record class BookResponse
    {
       public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime PublishedAt { get; set; }
        public IEnumerable<AuthorResponse> Authors { get; set; }
    }
}
