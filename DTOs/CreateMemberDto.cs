using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs
{
    public class CreateMemberDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
