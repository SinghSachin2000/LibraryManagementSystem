using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Services
{
    public interface IBookService
    {
        List<BookDto> GetAllBooks();
        BookDto? GetBookById(int id);
        BookDto CreateBook(CreateBookDto dto);
        BookDto? UpdateBook(int id, UpdateBookDto dto);
        bool DeleteBook(int id);
        string? ValidateBook(CreateBookDto dto);
        string? ValidateUpdateBook(int id, UpdateBookDto dto);
    }
}

