namespace Scales.Journal.Domain.Entities
{
    public class Transport : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string CarPlate { get; set; } = string.Empty;
        public int NumberOfAxles { get; set; }
        public float Weight { get; set; }

        public ICollection<Axles> Axles { get; set; } = new List<Axles>();
    }
}
