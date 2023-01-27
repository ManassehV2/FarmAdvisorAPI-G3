namespace FarmAdvisor.Models
{
    public class SensorStatistic
    {
        public Guid SensorStatisticId { get; set; }
        public Guid SensorId { get; set; }
        public DateTime Date { get; set; }
        public double averageTemperature { get; set; }
        public double GDD { get; set; }
    }
}