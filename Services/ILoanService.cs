using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Services
{
    public interface ILoanService
    {
        string? ValidateBorrow(BorrowBookDto dto);

        Loan BorrowBook(BorrowBookDto dto);
        string? ValidateReturn(int loanId);

        Loan? ReturnBook(int loanId);
        List<LoanDto> GetMemberLoans(int memberId);
    }
}