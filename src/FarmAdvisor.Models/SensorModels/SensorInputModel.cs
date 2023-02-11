using System.ComponentModel.DataAnnotations;

namespace FarmAdvisor.Models
{
    public class SensorInput
    {
        public Guid FieldId { get; set; }
        [Required]
        public string SerialNo { get; set; } = null!;
        public double GDD { get; set; }
        public int DefaultGDD { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public string InstallationDate { get; set; } = null!;
        public string LastCuttingDate { get; set; } = "0";
        public double BaseTemperature { get; set; }
    }
}