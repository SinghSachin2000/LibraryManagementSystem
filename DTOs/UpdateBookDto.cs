using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string ISBN { get; set; } = string.Empty;
        [Range(1000, 2100)]
        public int PublishedYear { get; set; }
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }
    }
}
