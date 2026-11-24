using LibraryManagement.Api.Models; 
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Services;

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

        //private readonly LibraryDbContext _context;

        //public BooksController(LibraryDbContext context)
        //{
        //    _context = context;
        //}
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        // IActionResult : method HTTP response return karega
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            return Ok(books);
            //return Ok(_books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            //var book = _books.FirstOrDefault(b => b.Id == id);
            var book = _bookService.GetBookById(id);

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
        public IActionResult CreateBook([FromBody] CreateBookDto  book)
        {
            var error = _bookService.ValidateBook(book);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            var createdBook = _bookService.CreateBook(book);
            return CreatedAtAction(
                nameof(GetBookById),
                new { id = createdBook.Id },
                createdBook
                );
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto book)
        {
            var error = _bookService.ValidateUpdateBook(id, book);

            if (error != null)
            {
                if (error == "Book not found")
                    return NotFound(new { message = error });

                return BadRequest(new { message = error });
            }

            var updatedBook = _bookService.UpdateBook(id, book);
            return Ok(updatedBook);
        }

        [HttpDelete("{id:int}")]
        public IActionResult deleteBook(int id)
        {
            //var book = _books.FirstOrDefault(b => b.Id == id);
            //var book = _context.Books.FirstOrDefault(b => b.Id == id);
            //if (book == null)
            //{
            //    return NotFound(new { message = "this book is not found" });
            //}
            ////_books.Remove(book);
            //_context.Books.Remove(book);
            //_context.SaveChanges();
            bool isDeleted = _bookService.DeleteBook(id);

            if (!isDeleted)
            {
                return NotFound(new { message = "Book not found" });
            }

            return NoContent();
            //204 No Content = operation successful hai, lekin response body mein kuch bhejne ki zaroorat nahi.
        }

    }
}
