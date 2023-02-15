namespace Scales.ReferenceBook.Domain.Entities
{
    public class ReferenceTransport : BaseEntity
    {
        public string Brand { get; set; } = string.Empty;
        public string CarPlate { get; set; } = string.Empty;
        public int NumberOfAxles { get; set; }
    }
}
