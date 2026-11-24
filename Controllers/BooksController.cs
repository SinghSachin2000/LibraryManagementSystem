using LibraryManagement.Api.Models; 
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        //in memory ------------------------------------------------------------
        //    private static readonly List<Book> _books = new()
        //    {
        //        //new Book : object initiliser 
        //         new Book
        //{
        //    Id = 1,
        //    Title = "Clean Code",
        //    Author = "Robert C. Martin",
        //    ISBN = "9780132350884",
        //    PublishedYear = 2008,
        //    TotalCopies = 5,
        //    AvailableCopies = 5
        //},

        //new Book
        //{
        //    Id = 2,
        //    Title = "The Pragmatic Programmer",
        //    Author = "Andrew Hunt",
        //    ISBN = "9780135957059",
        //    PublishedYear = 2019,
        //    TotalCopies = 3,
        //    AvailableCopies = 2
        //}
        //    };

        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // IActionResult : method HTTP response return karega
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                PublishedYear = b.PublishedYear,
                TotalCopies = b.TotalCopies,
                AvailableCopies = b.AvailableCopies
            }).ToList();
            return Ok(books);
            //return Ok(_books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            //var book = _books.FirstOrDefault(b => b.Id == id);
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

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
        public IActionResult AddBook([FromBody] CreateBookDto  book)
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
            //bool isAvailable = _books.Any(b => b.ISBN.Equals(book.ISBN, StringComparison.OrdinalIgnoreCase));
            bool isAvailable = _context.Books.Any(b => b.ISBN.ToLower() == book.ISBN.ToLower());
            if (isAvailable)
            {
                return BadRequest(new { message = "A book with this ISBN is already exists" });
            }

            var newbook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
            //book.Id = _books.Count == 0 ? 0 : _books.Max(b => b.Id) + 1;
            //book.Id = _context.Books.Count() == 0 ? 0 : _context.Books.Max(b => b.Id) + 1;

            //_books.Add(book);
            _context.Books.Add(newbook);
            _context.SaveChanges();

            var bookDto = new BookDto
            {
                Id = newbook.Id,
                Title = newbook.Title,
                Author = newbook.Author,
                ISBN = newbook.ISBN,
                PublishedYear = newbook.PublishedYear,
                TotalCopies = newbook.TotalCopies,
                AvailableCopies = newbook.AvailableCopies
            };
            return CreatedAtAction(
                nameof(GetBookById),
                new { id = newbook.Id },
                bookDto
                );
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
                return NotFound(new { message = $"Book with ID {id} was not found." });

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(new { message = "Title is required." });

            if (string.IsNullOrWhiteSpace(dto.Author))
                return BadRequest(new { message = "Author is required." });

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                return BadRequest(new { message = "ISBN is required." });

            if (dto.TotalCopies <= 0)
                return BadRequest(new { message = "TotalCopies must be greater than 0." });

            if (dto.AvailableCopies < 0 || dto.AvailableCopies > dto.TotalCopies)
                return BadRequest(new { message = "AvailableCopies must be between 0 and TotalCopies." });

            bool isbnExists = _context.Books.Any(b =>
                b.Id != id &&
                b.ISBN.ToLower() == dto.ISBN.ToLower());

            if (isbnExists)
                return BadRequest(new { message = "A book with this ISBN already exists." });

            existingBook.Title = dto.Title;
            existingBook.Author = dto.Author;
            existingBook.ISBN = dto.ISBN;
            existingBook.PublishedYear = dto.PublishedYear;
            existingBook.TotalCopies = dto.TotalCopies;
            existingBook.AvailableCopies = dto.AvailableCopies;

            _context.SaveChanges();

            return Ok(new BookDto
            {
                Id = existingBook.Id,
                Title = existingBook.Title,
                Author = existingBook.Author,
                ISBN = existingBook.ISBN,
                PublishedYear = existingBook.PublishedYear,
                TotalCopies = existingBook.TotalCopies,
                AvailableCopies = existingBook.AvailableCopies
            });
        }

        [HttpDelete("{id:int}")]
        public IActionResult deleteBook(int id)
        {
            //var book = _books.FirstOrDefault(b => b.Id == id);
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = "this book is not found" });
            }
            //_books.Remove(book);
            _context.Books.Remove(book);
            _context.SaveChanges();

            return NoContent();
            //204 No Content = operation successful hai, lekin response body mein kuch bhejne ki zaroorat nahi.
        }

    }
}
