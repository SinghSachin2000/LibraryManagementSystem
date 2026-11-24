using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public List<BookDto> GetAllBooks()
        {
            return _context.Books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies
                })
                .ToList();
        }

        public BookDto? GetBookById(int id)
        {
            return _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies
                })
                .FirstOrDefault();
        }
        public string? ValidateBook(CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return "Title is required";

            if (string.IsNullOrWhiteSpace(dto.Author))
                return "Author name is required";

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                return "ISBN number is required";

            if (dto.TotalCopies <= 0)
                return "Total copies must be greater than zero";

            if (dto.AvailableCopies > dto.TotalCopies || dto.AvailableCopies < 0)
                return "Available copies must be between 0 and TotalCopies";

            bool isbnExists = _context.Books.Any(b =>
                b.ISBN.ToLower() == dto.ISBN.ToLower());

            if (isbnExists)
                return "A book with this ISBN already exists";

            return null;
        }
        public BookDto CreateBook(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
        }
        public string? ValidateUpdateBook(int id, UpdateBookDto dto)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return "Book not found";

            if (string.IsNullOrWhiteSpace(dto.Title))
                return "Title is required";

            if (string.IsNullOrWhiteSpace(dto.Author))
                return "Author name is required";

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                return "ISBN number is required";

            if (dto.TotalCopies <= 0)
                return "Total copies must be greater than zero";

            if (dto.AvailableCopies > dto.TotalCopies || dto.AvailableCopies < 0)
                return "Available copies must be between 0 and TotalCopies";

            bool isbnExists = _context.Books.Any(b =>
                b.Id != id &&
                b.ISBN.ToLower() == dto.ISBN.ToLower());

            if (isbnExists)
                return "A book with this ISBN already exists";

            return null;
        }

        public BookDto? UpdateBook(int id, UpdateBookDto dto)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return null;

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.PublishedYear = dto.PublishedYear;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;

            _context.SaveChanges();

            return GetBookById(id);
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return false;

            _context.Books.Remove(book);
            _context.SaveChanges();

            return true;
        }

        public List<BookDto> GetBooks(string? author, bool? available, int page, int pageSize)
        {
            var query = _context.Books.AsQueryable();//database ke books table se query banate hain

            // Author filter
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b => b.Author.ToLower().Contains(author.ToLower()));
            }

            // Availability filter
            if (available == true)
            {
                query = query.Where(b => b.AvailableCopies > 0);
            }
            else if (available == false)
            {
                query = query.Where(b => b.AvailableCopies == 0);
            }

            // Pagination
            //OrderBy : Books ko ID ke order mein arrange karta hai.
            //skip and takeRequired page ki books select karta hai.
            return query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    PublishedYear = b.PublishedYear,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies
                })
                .ToList();
        }
    }
}