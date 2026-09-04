using LibraryAPI.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private static readonly ConcurrentDictionary<int, Book> _books = new();
        private static int _nextId = 1;

        static InMemoryBookRepository()
        {
            SeedData();
        }

        private static void SeedData()
        {
            var initialBooks = new List<Book>
            {
                new Book { Id = 1, Title = "1984", Author = "George Orwell", AuthorId = 1, Category = "Dystopian", PublishedYear = 1949, ISBN = "978-0451524935" },
                new Book { Id = 2, Title = "Animal Farm", Author = "George Orwell", AuthorId = 1, Category = "Classic Fiction", PublishedYear = 1945, ISBN = "978-0451526342" },
                new Book { Id = 3, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", AuthorId = 2, Category = "Fantasy", PublishedYear = 1997, ISBN = "978-0590353427" },
                new Book { Id = 4, Title = "Clean Code", Author = "Robert C. Martin", AuthorId = 3, Category = "Software Engineering", PublishedYear = 2008, ISBN = "978-0132350884" }
            };

            foreach (var b in initialBooks)
            {
                _books[b.Id] = b;
            }
            _nextId = 5;
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Book>>(_books.Values.OrderBy(b => b.Id).ToList());
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            _books.TryGetValue(id, out var book);
            return Task.FromResult(book);
        }

        public Task<Book> AddAsync(Book book)
        {
            book.Id = _nextId++;
            _books[book.Id] = book;
            return Task.FromResult(book);
        }

        public Task<bool> UpdateAsync(Book book)
        {
            if (!_books.ContainsKey(book.Id)) return Task.FromResult(false);
            _books[book.Id] = book;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(_books.TryRemove(id, out _));
        }
    }
}
