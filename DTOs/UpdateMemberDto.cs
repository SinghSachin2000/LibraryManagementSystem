using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs
{
    public class UpdateMemberDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
