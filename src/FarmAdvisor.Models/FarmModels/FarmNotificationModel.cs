namespace FarmAdvisor.Models
{
    public class FarmNotification
    {
        public static class NotificationType
        {
            public static readonly string LowBattery = "LowBattery";
            public static readonly string GddExceeded = "GddExceeded";
            
        };

        public Guid FieldId { get; set; }
        public string FieldName { get; set; } = null!;
        public Guid SensorId { get; set; }
        public string SensorSerialNo { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}