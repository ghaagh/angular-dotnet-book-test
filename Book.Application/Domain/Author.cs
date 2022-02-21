namespace Book.Application.Domain
{
    public class Author : IAggregateRoot
    {
        public Author(string name)
        {
            Name = name;
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public virtual ICollection<AuthorBook>? AuthorBooks { get; set; }
        public bool IsDeleted { get; private set; }
        public void Delete() => IsDeleted = true;
        public void SetName(string name) => Name = name;
    }
}
