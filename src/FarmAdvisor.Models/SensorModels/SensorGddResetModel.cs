using System.ComponentModel.DataAnnotations;
namespace FarmAdvisor.Models
{
    public class SensorGddReset
    {
        public Guid SensorGddResetId { get; set; }
        public Guid SensorId { get; set; }
        public DateTime resetDate { get; set; }
    }
}