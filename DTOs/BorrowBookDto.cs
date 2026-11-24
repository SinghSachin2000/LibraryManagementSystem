using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs
{
    public class BorrowBookDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MemberId { get; set; }
    }
}