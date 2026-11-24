using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public IActionResult BorrowBook([FromBody] BorrowBookDto dto)
        {
            // Validation
            var error = _loanService.ValidateBorrow(dto);

            if (error != null)
            {
                if (error == "Member not found" || error == "Book not found")
                    return NotFound(new { message = error });

                return BadRequest(new { message = error });
            }

            // Borrow operation
            var loan = _loanService.BorrowBook(dto);

            return Ok(loan);
        }

        [HttpPost("{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            // Return validation
            var error = _loanService.ValidateReturn(id);

            if (error != null)
            {
                if (error == "Loan not found")
                    return NotFound(new { message = error });

                return BadRequest(new { message = error });
            }

            // Return operation
            var loan = _loanService.ReturnBook(id);

            return Ok(loan);
        }
    }
}