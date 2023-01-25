namespace FarmAdvisor.Models
{
    public class Field
    {
        public Guid? FieldId { get; set; }
        public Guid FarmId { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; } = null!;
        public int Altitude { get; set; } = 0;
        public string Polygon { get; set; } = null!;
    }
}