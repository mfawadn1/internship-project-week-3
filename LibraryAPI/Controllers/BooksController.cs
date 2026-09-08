using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Publicly accessible for browsing catalog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // Publicly accessible for viewing details
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // Requires any authenticated user (Admin or User)
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            var createdBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        // Requires any authenticated user (Admin or User)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _bookService.UpdateBookAsync(book);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Strictly restricted to users with the 'Admin' role
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
