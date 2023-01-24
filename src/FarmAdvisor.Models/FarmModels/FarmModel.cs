using System.ComponentModel.DataAnnotations;

namespace FarmAdvisor.Models
{
    public class Farm
    {
        // [Required]
        public Guid? FarmId { get; set; }
        // [Required]
        public Guid? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}