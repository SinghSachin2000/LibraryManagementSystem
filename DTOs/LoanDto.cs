namespace LibraryManagement.Api.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowedOn { get; set; }
        public DateTime DueOn { get; set; }
        public DateTime? ReturnedOn { get; set; }
    }
}