using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book - Author relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.AuthorEntity)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Book - Category Many-to-Many
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategories"));

            // Seed Initial Authors and Books
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, FullName = "George Orwell", Bio = "English novelist" },
                new Author { AuthorId = 2, FullName = "J.K. Rowling", Bio = "British fantasy author" },
                new Author { AuthorId = 3, FullName = "Robert C. Martin", Bio = "Uncle Bob - Clean Architecture" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Classic Fiction" },
                new Category { CategoryId = 2, CategoryName = "Dystopian" },
                new Category { CategoryId = 3, CategoryName = "Fantasy" },
                new Category { CategoryId = 4, CategoryName = "Software Engineering" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Author = "George Orwell", AuthorId = 1, Category = "Dystopian", PublishedYear = 1949, ISBN = "978-0451524935" },
                new Book { Id = 2, Title = "Animal Farm", Author = "George Orwell", AuthorId = 1, Category = "Classic Fiction", PublishedYear = 1945, ISBN = "978-0451526342" },
                new Book { Id = 3, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", AuthorId = 2, Category = "Fantasy", PublishedYear = 1997, ISBN = "978-0590353427" },
                new Book { Id = 4, Title = "Clean Code", Author = "Robert C. Martin", AuthorId = 3, Category = "Software Engineering", PublishedYear = 2008, ISBN = "978-0132350884" }
            );
        }
    }
}
