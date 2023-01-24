using System.ComponentModel.DataAnnotations;

namespace FarmAdvisor.Models
{
    public class User
    {
        [Required]
        public Guid? userId { get; set; }
        public string name { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string email { get; set; } = null!;
        [Required]
        public string? passwordHash {get; set;}
    }
}