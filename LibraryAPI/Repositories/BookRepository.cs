using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.AuthorEntity)
                .Include(b => b.Categories)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.AuthorEntity)
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existing = await _context.Books.FindAsync(book.Id);
            if (existing == null) return false;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Category = book.Category;
            existing.AuthorId = book.AuthorId;
            existing.PublishedYear = book.PublishedYear;
            existing.ISBN = book.ISBN;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Books.FindAsync(id);
            if (existing == null) return false;

            _context.Books.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
