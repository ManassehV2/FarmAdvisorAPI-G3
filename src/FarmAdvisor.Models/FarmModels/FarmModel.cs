namespace FarmAdvisor.Models
{
    public class Farm
    {
        public Guid? FarmId { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}