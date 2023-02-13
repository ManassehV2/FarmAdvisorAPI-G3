using System.ComponentModel.DataAnnotations;
namespace FarmAdvisor.Models
{
    public class Sensor : SensorInput
    {
        [Required]
        public Guid? SensorId { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        public bool BatteryStatus { get; set; } = true;
        public string LastCommunication { get; set; } = "0";
        public DateTime? cuttingDate { get; set; }
    }
}