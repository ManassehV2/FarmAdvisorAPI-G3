using System.ComponentModel.DataAnnotations;
namespace FarmAdvisor.Models
{
    public class FieldUpdate
    {
        public string? Name { get; set; }
        public int? Altitude { get; set; }
        public string? Polygon { get; set; }
    }
}