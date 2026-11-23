using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Api.Models; 

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> _books = new()
        {
            //new Book : object initiliser 
             new Book
    {
        Id = 1,
        Title = "Clean Code",
        Author = "Robert C. Martin",
        ISBN = "9780132350884",
        PublishedYear = 2008,
        TotalCopies = 5,
        AvailableCopies = 5
    },

    new Book
    {
        Id = 2,
        Title = "The Pragmatic Programmer",
        Author = "Andrew Hunt",
        ISBN = "9780135957059",
        PublishedYear = 2019,
        TotalCopies = 3,
        AvailableCopies = 2
    }
        };

        // IActionResult : method HTTP response return karega
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(_books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new
                {
                    message = $"Book with ID {id} was not found."
                });
            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return BadRequest(new { message = "title is required" });
            }
            if (string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest(new { message = "author name is requirred" });
            }
            if (string.IsNullOrWhiteSpace(book.ISBN))
            {
                return BadRequest(new { message = "ISBN number is required" });
            }
            if (book.TotalCopies <= 0)
            {
                return BadRequest(new { message = "total copies must be greater than zero" });
            }
            if (book.AvailableCopies > book.TotalCopies || book.AvailableCopies < 0)
            {
                return BadRequest(new { message = "available copy must be lower or equal to the Total copy and must be greater than zero" });

            }
            bool isAvailable = _books.Any(b => b.ISBN.Equals(book.ISBN, StringComparison.OrdinalIgnoreCase));

            if (isAvailable)
            {
                return BadRequest(new { message = "A book with this ISBN is already exists" });
            }

            book.Id = _books.Count == 0 ? 0 : _books.Max(b => b.Id) + 1;

            _books.Add(book);
            return CreatedAtAction(
                nameof(GetBookById),
                new { id = book.Id },
                book
                );
        }

        [HttpPut("{id:int}")]
        public IActionResult updateBook(int id, [FromBody] Book updatedBook)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound(new { message = "book is not found" });
            }
            if (string.IsNullOrWhiteSpace(updatedBook.Title))
            {
                return BadRequest(new { message = "title is required" });
            }
            if (string.IsNullOrWhiteSpace(updatedBook.Author))
            {
                return BadRequest(new { message = "author name is requirred" });
            }
            if (string.IsNullOrWhiteSpace(updatedBook.ISBN))
            {
                return BadRequest(new { message = "ISBN number is required" });
            }
            if (updatedBook.TotalCopies <= 0)
            {
                return BadRequest(new { message = "total copies must be greater than zero" });
            }
            if (updatedBook.AvailableCopies > updatedBook.TotalCopies || updatedBook.AvailableCopies < 0)
            {
                return BadRequest(new { message = "available copy must be lower or equal to the Total copy and must be greater than zero" });

            }
            bool isAvailable = _books.Any(b => b.Id !=id && b.ISBN.Equals(updatedBook.ISBN, StringComparison.OrdinalIgnoreCase));

            if (isAvailable)
            {
                return BadRequest(new { message = "A book with this ISBN is already exists" });
            }
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.TotalCopies = updatedBook.TotalCopies;
            existingBook.AvailableCopies = updatedBook.AvailableCopies;

            return Ok(existingBook);
        }

        [HttpDelete("{id:int}")]
        public IActionResult deleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = "this book is not found" });
            }
            _books.Remove(book);
            return NoContent();
            //204 No Content = operation successful hai, lekin response body mein kuch bhejne ki zaroorat nahi.
        }

    }
}
