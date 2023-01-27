
namespace FarmAdvisor.Models
{
    public class SensorUpdate
    {
        public Guid? FieldId { get; set; }
        public string? SerialNo { get; set; }
        public double? GDD { get; set; }
        public int? DefaultGDD { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public string? InstallationDate { get; set; }
        public string? LastCuttingDate { get; set; }
        public double? BaseTemperature { get; set; }
    }
}