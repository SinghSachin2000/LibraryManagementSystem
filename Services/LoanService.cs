using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Services
{
    public class LoanService : ILoanService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<LoanService> _logger;

        public LoanService(LibraryDbContext context, ILogger<LoanService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string? ValidateBorrow(BorrowBookDto dto)
        {
            // 1. Member exist karta hai ya nahi
            var member = _context.Members
                .FirstOrDefault(m => m.Id == dto.MemberId);

            if (member == null)
                return "Member not found";

            // 2. Member active hai ya nahi
            if (!member.IsActive)
                return "Inactive members cannot borrow books";

            // 3. Member ke active loans count karo
            int activeLoans = _context.Loans.Count(l =>
                l.MemberId == dto.MemberId &&
                l.ReturnedOn == null);

            if (activeLoans >= 3)
                return "Member cannot have more than 3 active loans";

            // 4. Book exist karti hai ya nahi
            var book = _context.Books
                .FirstOrDefault(b => b.Id == dto.BookId);

            if (book == null)
                return "Book not found";

            // 5. Book available hai ya nahi
            if (book.AvailableCopies <= 0)
                return "Book is not available";

            return null;
        }

        public Loan BorrowBook(BorrowBookDto dto)
        {
            var book = _context.Books
                .First(b => b.Id == dto.BookId);

            var borrowedOn = DateTime.Now;

            var loan = new Loan
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowedOn = borrowedOn,
                DueOn = borrowedOn.AddDays(14),
                ReturnedOn = null
            };

            // Loan database mein save karo
            _context.Loans.Add(loan);

            // Available copies 1 kam karo
            book.AvailableCopies--;

            // Dono changes database mein save karo
            _context.SaveChanges();
            _logger.LogInformation(
             "Book {BookId} borrowed by Member {MemberId}. LoanId: {LoanId}",
             loan.BookId,
             loan.MemberId,
             loan.Id);

            return loan;
        }

        public string? ValidateReturn(int loanId)
        {
            // 1. Loan exist karta hai ya nahi
            var loan = _context.Loans
                .FirstOrDefault(l => l.Id == loanId);

            if (loan == null)
                return "Loan not found";

            // 2. Book already return toh nahi hui
            if (loan.ReturnedOn != null)
                return "Book has already been returned";

            return null;
        }
        public Loan? ReturnBook(int loanId)
        {
            var loan = _context.Loans
                .FirstOrDefault(l => l.Id == loanId);

            if (loan == null)
                return null;

            var book = _context.Books
                .FirstOrDefault(b => b.Id == loan.BookId);

            if (book == null)
                return null;

            // Return date set karo
            loan.ReturnedOn = DateTime.Now;

            // Book stock 1 increase karo
            book.AvailableCopies++;

            // Database update
            _context.SaveChanges();
            _logger.LogInformation(
                 "Loan {LoanId} returned. BookId: {BookId}, MemberId: {MemberId}",
                  loan.Id,
                  loan.BookId,
                  loan.MemberId);

            return loan;
        }

        public List<LoanDto> GetMemberLoans(int memberId)
        {
            return _context.Loans
                .Where(l => l.MemberId == memberId)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    BookId = l.BookId,
                    MemberId = l.MemberId,
                    BorrowedOn = l.BorrowedOn,
                    DueOn = l.DueOn,
                    ReturnedOn = l.ReturnedOn
                })
                .ToList();
        }
    }
}