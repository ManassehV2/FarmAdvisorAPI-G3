using System.ComponentModel.DataAnnotations;
namespace FarmAdvisor.Models
{
    public class FarmUpdate
    {
        public string? Name { get; set; }
        public string? Postcode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}