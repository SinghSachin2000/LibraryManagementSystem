using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
