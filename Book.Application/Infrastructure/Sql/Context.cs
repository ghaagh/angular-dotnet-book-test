using Book.Application.Infrastructure.Sql.Repository;
using Microsoft.EntityFrameworkCore;
namespace Book.Application.Infrastructure.Sql
{
    public class Context: DbContext
    {

        public Context() : base() { }
        public Context(DbContextOptions<Context> options) : base(options) {
            base.ChangeTracker.StateChanged += new EntityStateChangeHandler().StateChanged;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Domain.Book>()
                .HasMany(c => c.AuthorBooks)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Domain.Author>()
                .HasMany(c => c.AuthorBooks)
                .WithOne(c => c.Author)
                .HasForeignKey(v => v.AuthorId);

            modelBuilder.Entity<Domain.Book>()
                .Property(c => c.BookTitle)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Domain.Book>()
                .Property(c => c.ISBN)
                .IsRequired()
                .HasMaxLength(15);

            modelBuilder.Entity<Domain.Book>()
                .Property(c => c.PublishedAt)
                .IsRequired();

            modelBuilder.Entity<Domain.Author>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Domain.AuthorBook>()
                .HasKey(c => new { c.BookId, c.AuthorId });

        }
        public async override Task<int> SaveChangesAsync(CancellationToken token= default)
        {

            return await base.SaveChangesAsync(token);
        }
        public virtual DbSet<Domain.Book> Books { get; set; }
        public virtual DbSet<Domain.Author> Authors { get; set; }
    }
}
