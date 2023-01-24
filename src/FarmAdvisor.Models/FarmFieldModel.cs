namespace FarmAdvisor.Models
{
    public class FarmFieldModel
    {
        public Guid FieldId { get; set; }
        public Guid FarmId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Altitude { get; set; }
        public string Polygon { get; set; }  = null!;
    }
}