using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public DateTime BorrowedOn { get; set; }

        public DateTime DueOn { get; set; }

        public DateTime? ReturnedOn { get; set; }
    }
}