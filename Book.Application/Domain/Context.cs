
using Book.Application.Domain;
using Book.Application.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Book.Application.Domain
{
    public class Context: DbContext
    {
        public Context() : base() { }
        public Context(DbContextOptions<Context> options) : base(options) {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Domain.Book>()
                .HasMany(c => c.AuthorBooks)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Author>()
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

        public virtual DbSet<Domain.Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BookHistory> BookHistories { get; set; }


    }
}
