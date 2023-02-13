using System.ComponentModel.DataAnnotations;
namespace FarmAdvisor.Models
{
    public class User
    {
        [Required]
        public Guid? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        [Required]
        public string? PasswordHash {get; set;}
    }
}